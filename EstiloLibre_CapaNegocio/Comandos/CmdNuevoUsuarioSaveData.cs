using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdNuevoUsuarioSaveData
        : ComandoBase, IRequest<int>
    {
        public DTOs.NuevoUsuarioSaveDataDTO Usuario { get; set; }
        public CmdNuevoUsuarioSaveData(DTOs.NuevoUsuarioSaveDataDTO usuarioSaveData) : base()
        {
            this.Usuario = usuarioSaveData;
        }
    }

    public class PcmdNuevoUsuarioSaveData
        : ProcesadorDeComandoBase, IRequestHandler<CmdNuevoUsuarioSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdNuevoUsuarioSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdNuevoUsuarioSaveData comando, CancellationToken cancellationToken)
        {
            Usuario? usuario;

            try
            {
                //Envolver todo el proceso en una transacción.
                con.BeginTrans();

                //Comprobar uso del login.
                usuario = con.CargarUsuario(comando.Usuario.Login);
                if (usuario != null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_LoginEnUso");
                }

                //Comprobar uso del correo electrónico.
                usuario = con.CargarUsuarioPorCorreo(comando.Usuario.CorreoE);
                if (usuario != null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_CorreoEnUso");
                }

                //Crear usuario.
                usuario = con.CrearUsuario();

                //Transferir propiedades del DTO al objeto de BD.
                usuario.Login = comando.Usuario.Login;
                usuario.Nombre = comando.Usuario.Nombre;
                usuario.Apellido1 = comando.Usuario.Apellido1;
                usuario.Apellido2 = comando.Usuario.Apellido2;
                usuario.CorreoE = comando.Usuario.CorreoE;
                usuario.IdiomaId = comando.Usuario.IdiomaId;
                usuario.Publico = false; //Falso por defecto
                usuario.Telefono = comando.Usuario.Telefono;                      
                usuario.FechaNacimiento = comando.Usuario.FechaNacimiento;
                usuario.Contraseña = UtilsVarios.GenerarHash(comando.Usuario.Contraseña);

                //Guardar todos los cambios recibidos.
                usuario.Guardar();

                //Confirmar transacción.
                con.CommitTrans();

                //Devolver id.
                return usuario.Id;
            }
            catch
            {
                con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}
