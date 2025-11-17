using DocumentFormat.OpenXml.Office2010.CustomUI;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySql.Data.MySqlClient;
using System.Data;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuarioDatosIniciales : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuarioDatosIniciales(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Usuarios} p
                WHERE p.Id = @personaId;

                SELECT p.* 
                FROM {TablasBD.Permisos} p 
                INNER JOIN {TablasBD.PermisosUsuarios} pu ON p.Id = pu.PermisoId AND pu.UsuarioId = @iUsuarioId;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { "Usuario", "Permisos" };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            DataSet datos;
            DataTable tabla;
            Usuario usuario;
            List<string> permisos;

            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            datos = this.EjecutarConsulta();

            usuario = new Usuario();
            usuario.IniciarListaPermisos();

            // Asignar datos del usuario
            if (this.TablaTieneDatos("Usuario"))
            {
                tabla = this.GetTabla("Usuario")!;

                foreach (DataRow fila in tabla.Rows)
                {
                    usuario = new Usuario()
                    {
                        Login = UtilsConversion.GetString(fila["Login"])!,
                        Contraseña = UtilsConversion.GetString(fila["Contraseña"])!,
                        Nombre = UtilsConversion.GetString(fila["Nombre"])!,
                        Apellido1 = UtilsConversion.GetString(fila["Apellido1"])!,
                        Apellido2 = UtilsConversion.GetString(fila["Apellido2"]),
                        FechaNacimiento = UtilsConversion.GetDateTime(fila["FechaNacimiento"]),
                        CorreoE = UtilsConversion.GetString(fila["CorreoE"]),
                        Telefono = UtilsConversion.GetInt(fila["Telefono"]),
                        FechaBaja = UtilsConversion.GetDateTime(fila["FechaBaja"]),
                        Publico = UtilsConversion.GetBool(fila["Publico"])
                    };
                }
            }

            // Asignar permisos
            if (this.TablaTieneDatos("Permisos"))
            {
                tabla = this.GetTabla("Permisos")!;

                permisos = new List<string>();

                foreach (DataRow fila in tabla.Rows)
                {
                    permisos.Add(UtilsConversion.GetString(fila["Codigo"])!);
                }

                this.Usuario.Permisos = permisos;
            }            
        }

        #endregion
    }
}
