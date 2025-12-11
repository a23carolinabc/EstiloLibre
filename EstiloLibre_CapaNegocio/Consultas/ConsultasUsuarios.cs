using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuarios.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuarios
    {
        #region ***** PROPIEDADES INTERNAS ***** 
        public Conexion _con;
        public UsuariosDAO _dao;
        private ServicioCombos _servicioCombos;
        private ServicioAlmacenamiento _servicioAlmacenamiento;
        #endregion

        #region ***** CONSTRUCTOR ***** 
        public ConsultasUsuarios(Conexion con, ServicioCombos servicioCombos, ServicioAlmacenamiento servicioAlmacenamiento)
        { 
            this._con = con;
            this._dao = new UsuariosDAO(_con);
            this._servicioCombos = servicioCombos;
            this._servicioAlmacenamiento = servicioAlmacenamiento;
        }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<UsuarioShowDataDTO> GetDatosShowData(int iUsuarioId)
        {
            CDUsuariosShowData cd;
            UsuarioShowDataDTO dto;


            cd = new CDUsuariosShowData(this._con);
            cd.Cargar(iUsuarioId);

            dto = await this.GetDatosParaShowData(cd);
            return dto;
        }

        public UsuarioAddNewDTO GetDatosAddNew()
        {
            CDUsuariosAddNew cd;
            UsuarioAddNewDTO dto;


            cd = new CDUsuariosAddNew(this._con);
            cd.Cargar();

            dto = this.GetDatosParaAddNew(cd);
            return dto;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS ***** 

        public Usuario? GetUsuario(int usuarioId)
        {
            return this._dao.CargarUsuario(usuarioId);
        }

        public Usuario? GetUsuarioPorLogin(string login)
        {
            return this._dao.CargarUsuarioPorLogin(login);
        }

        private async Task<UsuarioShowDataDTO> GetDatosParaShowData(CDUsuariosShowData cd)
        {
            UsuarioShowDataDTO objeto;
            Adjuntos adjuntos;

            objeto = new();
            objeto.Usuario = new(cd.Usuario);
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);
            if (objeto.Usuario.Id > 0)
            {
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, objeto.Usuario.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    objeto.Usuario.FotoBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }
            }
            return objeto;
        }

        private UsuarioAddNewDTO GetDatosParaAddNew(CDUsuariosAddNew cd)
        {
            UsuarioAddNewDTO objeto;

            objeto = new();
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);
            
            return objeto;
        }
        #endregion
    }
}
