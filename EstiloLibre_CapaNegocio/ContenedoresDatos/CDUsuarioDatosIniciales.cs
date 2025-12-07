using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
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
                WHERE p.Id = @iUsuarioId;

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
            DataTable tabla;
            List<string> permisos;

            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            this.Usuario = new Usuario();
            this.Usuario.IniciarListaPermisos();

            // Asignar datos del usuario
            if (this.TablaTieneDatos("Usuario"))
            {
                tabla = this.GetTabla("Usuario")!;

                foreach (DataRow fila in tabla.Rows)
                {
                    this.Usuario.Id = UtilsConversion.GetInt(fila["Id"]) ?? 0;
                    this.Usuario.Login = UtilsConversion.GetString(fila["Login"])!;
                    this.Usuario.Contraseña = UtilsConversion.GetString(fila["Contraseña"])!;
                    this.Usuario.Nombre = UtilsConversion.GetString(fila["Nombre"])!;
                    this.Usuario.Apellido1 = UtilsConversion.GetString(fila["Apellido1"])!;
                    this.Usuario.Apellido2 = UtilsConversion.GetString(fila["Apellido2"]);
                    this.Usuario.FechaNacimiento = UtilsConversion.GetDateTime(fila["FechaNacimiento"]);
                    this.Usuario.CorreoE = UtilsConversion.GetString(fila["CorreoE"]);
                    this.Usuario.Telefono = UtilsConversion.GetInt(fila["Telefono"]);
                    this.Usuario.FechaBaja = UtilsConversion.GetDateTime(fila["FechaBaja"]);
                    this.Usuario.Publico = UtilsConversion.GetBool(fila["Publico"]);
                    this.Usuario.IdiomaId = UtilsConversion.GetInt(fila["IdiomaId"])??0;
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
