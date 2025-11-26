using EstiloLibre_CapaNegocio.Configuracion;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public partial class Conexion
    {
        #region ***** PROPIEDADES INTERNAS *****

        private DatosSesion _datosDeSesion;
        private static object _objetoDeBloqueo = new Object();

        #endregion

        #region ***** MÉTODOS DE PROPIEDAD *****

        public ConexionBD ConexionBD { get; set; }
        public UsuarioAutenticadoDTO UsuarioAutenticado { get; set; }
        public EstiloLibre_CapaNegocio.Configuracion.Configuracion ConfiguracionEstiloLibre { get; set; }
        public bool bTransaccionPUIniciada { get; set; }

        public int IdiomaActualId
        {
            get
            {
                if (this._datosDeSesion != null && this._datosDeSesion.Idioma != null)
                {
                    return this._datosDeSesion.Idioma.Id;
                }
                else if (this.UsuarioAutenticado.IdiomaActualId > 0)
                {
                    return this.UsuarioAutenticado.IdiomaActualId;
                }
                else
                {
                    throw new CapaNegocioException("No se ha establecido el idioma actual en el objeto Conexion.");
                }
            }
        }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Conexion(EstiloLibre_CapaNegocio.Configuracion.Configuracion config)
        {
            Configuracion cfg;

            // Almacenar una copia de la configuración.
            this.bTransaccionPUIniciada = false;
            if ((config == null))
                config = new EstiloLibre_CapaNegocio.Configuracion.Configuracion();
            this.ConfiguracionEstiloLibre = config;

            // Configuración de la capa de acceso a base de datos.
            cfg = new Configuracion(config.CadenaDeConexion);
            cfg.iSqlQueryTimeOut = config.TimeOutConsultasSql;
            //cfg.UsarControlConcurrencia = false;

            // Crear una conexión con la capa de acceso a base de datos (cerrada).
            this.ConexionBD = new ConexionBD(cfg);
        }
        #endregion

        #region "***** MÉTODOS PÚBLICOS *****"

        internal ConexionBD GetConexionBD()
        {
            return this.ConexionBD;
        }

        // Establece conexión con la capa de acceso a datos.
        public void Conectar()
        {
            this.ConexionBD.Conectar();
        }

        public void Desconectar()
        {
            this.ConexionBD.Desconectar();
        }

        public void BeginTrans(bool bContinuar = false)
        {
            if ((!this.bTransaccionPUIniciada))
                this.ConexionBD.BeginTrans(bContinuar);
        }

        public void CommitTrans()
        {
            if ((!this.bTransaccionPUIniciada))
                this.ConexionBD.CommitTrans();
        }

        public void BeginTransPU()
        {
            this.bTransaccionPUIniciada = true;
            this.ConexionBD.BeginTrans();
        }

        public void RollBackTrans()
        {
            if ((!this.bTransaccionPUIniciada))
                this.ConexionBD.RollBackTrans();
        }

        public void RollBackTransPU()
        {
            this.ConexionBD.RollBackTrans();
            this.bTransaccionPUIniciada = false;
        }

        protected void GetValorColumna()
        {

        }

        public DatosSesionDTO GetDatosDeSesion(UsuarioAutenticadoDTO usuario)
        {
            DatosSesionDTO datosDeSesion;

            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            datosDeSesion = new DatosSesionDTO()
            {
                CodigoIdioma = usuario.CodigoIdiomaActual
            };

            return datosDeSesion;
        }
        #endregion

        #region "***** MÉTODOS PRIVADOS *****"      

        public void EstablecerUsuarioActual(UsuarioAutenticadoDTO usuario)
        {
            //Almacenar el usuario actual y los datos de sesión.
            this.UsuarioAutenticado = usuario;
        }

        public bool UsuarioActualCumplePermisos(IEnumerable<string> lstCodigosPermisos)
        {
            if (lstCodigosPermisos == null || !lstCodigosPermisos.Any())
            {
                return false;
            }

            if (this.UsuarioAutenticado.Permisos == null || !this.UsuarioAutenticado.Permisos.Any())
            {
                return false;
            }

            foreach (string permiso in lstCodigosPermisos)
            {
                if (!this.UsuarioAutenticado.Permisos.Contains(permiso))
                {
                    return false;
                }
            }
            return true;
        }

        public bool UsuarioActualCumplePermisos(string[] lstCodigosPermisos)
        {
            return this.UsuarioActualCumplePermisos(lstCodigosPermisos.ToList());
        }

        public void EstablecerDatosSesion(DatosSesionDTO datosSesionDTO)
        {
            Idioma idioma;

            //Comprobar que el identificador indicado se corresponda con un idioma soportado por el sistema.
            idioma = this.CargarIdioma(datosSesionDTO.CodigoIdioma);
            if (idioma == null)
            {
                throw new ReglaNegocioException("Usuarios_RN_002", datosSesionDTO.CodigoIdioma);
            }

            //Crear el objeto interno de datos de sesión.
            this._datosDeSesion = new DatosSesion()
            {
                Idioma = idioma,
            };
        }

        #endregion
    }
}