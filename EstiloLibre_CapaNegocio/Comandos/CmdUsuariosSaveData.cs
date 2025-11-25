using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdUsuariosSaveData
        : ComandoBase, IRequest<int>
    {
        public Dtos.UsuarioSaveData UsuarioSaveData { get; set; }
        public CmdUsuariosSaveData(Dtos.UsuarioSaveData usuarioSaveData) : base([AccesoBD.CodigosPermisos.MOD_Usuarios])
        {
            this.UsuarioSaveData = usuarioSaveData;
        }
    }

    public class PcmdUsuariosSaveData
        : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosSaveData, int>
    {
        #region ***** CONSTRUCTORES *****

        public PcmdUsuariosSaveData(Conexion con) : base(con) { }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public Task<int> Handle(CmdUsuariosSaveData comando, CancellationToken cancellationToken)
        {
            Usuario? usuario;

            base.VericarPermisos(comando);

            try
            {
                //Envolver todo el proceso en una transacción.
                con.BeginTrans();

                //Buscar si el aviso ya estaba registrado en BD.
                if (comando.UsuarioSaveData.Usuario.Id > 0)
                {
                    usuario = con.CargarUsuario(comando.UsuarioSaveData.Usuario.Id);
                    if (usuario == null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                    }
                }
                else
                {
                    usuario = con.CargarUsuario(comando.UsuarioSaveData.Usuario.Login);
                    if (usuario != null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_LoginEnUso");
                    }
                    usuario = con.CrearUsuario();
                }

                //Transferir propiedades del DTO al objeto de BD.
                usuario.Id = comando.UsuarioSaveData.Usuario.Id;
                usuario.Login = comando.UsuarioSaveData.Usuario.Login;
                usuario.Nombre = comando.UsuarioSaveData.Usuario.Nombre;
                usuario.Apellido1 = comando.UsuarioSaveData.Usuario.Apellido1;
                usuario.Apellido2 = comando.UsuarioSaveData.Usuario.Apellido2;
                usuario.CorreoE = comando.UsuarioSaveData.Usuario.CorreoE;
                usuario.IdiomaId = comando.UsuarioSaveData.Usuario.IdiomaId;
                usuario.Publico = true;
                if (!comando.UsuarioSaveData.Usuario.Activo)
                {
                    usuario.FechaBaja = DateTime.Now;
                }                                
                if(comando.UsuarioSaveData.Usuario.Contraseña != null)
                {
                    usuario.Contraseña = UtilsVarios.GenerarHash(comando.UsuarioSaveData.Usuario.Contraseña);
                }

                //Guardar todos los cambios recibidos.
                usuario.Guardar();                    

                //Confirmar transacción.
                con.CommitTrans();

                //Devolver id.
                return Task.FromResult(usuario.Id);
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
