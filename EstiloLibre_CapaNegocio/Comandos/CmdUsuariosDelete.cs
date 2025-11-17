using MediatR;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public class CmdUsuariosDelete 
        :ComandoBase, IRequest
    {
        public int UsuarioId { get; set; }
        public CmdUsuariosDelete(int iUsuarioId) : base([AccesoBD.CodigosPermisos.MOD_Usuarios])
        {
            this.UsuarioId = iUsuarioId;
        }
    }

    public class PcmdUsuariosDelete 
        : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosDelete>
    {
        public PcmdUsuariosDelete(Conexion con)
            : base(con)
        {
        }

        public Task Handle(CmdUsuariosDelete comando, CancellationToken cancellationToken)
        {
            Usuario Usuario;

            base.VericarPermisos(comando);

            try
            {
                con.BeginTrans();

                Usuario = con.CargarUsuario(comando.UsuarioId);
                if (Usuario is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }                

                //Eliminar usuario.
                Usuario.Eliminar();

                //Confirmar la transacción.
                con.CommitTrans();

                //Devolver el resultado de la ejecución del comando.
                return Task.FromResult(Unit.Value);
            }
            catch
            {
                //Fallo detectado. Deshacer transacción y relanzar la excepción.
                con.RollBackTrans();
                throw;
            }    
        }   
    }
}
