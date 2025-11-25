using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos;

public class CmdActualizarDatosSesion 
    : ComandoBase, IRequest<Unit>
{
    public required string NuevoCodigoIdioma { get; set; }
}

public class PcmdActualizarDatosSesion 
    : ProcesadorDeComandoBase, IRequestHandler<CmdActualizarDatosSesion, Unit>
{
    #region ***** CONSTRUCTORES *****

    public PcmdActualizarDatosSesion(Conexion con) 
        : base(con) 
    { 
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    public async Task<Unit> Handle(CmdActualizarDatosSesion comando, CancellationToken cancellationToken)
    {
        Usuario usuarioActual;

        // No es necesario comprobar permisos porque todos los usuarios pueden actualizar sus propios datos.
                
        try
        {
            // Envolver todo el proceso en una transacción.
            con.BeginTrans();

            // Realizar la actualización.
            usuarioActual = con.CargarUsuarioActual();
            usuarioActual.IdiomaId = con.CargarIdioma(comando.NuevoCodigoIdioma).Id;
            usuarioActual.Guardar();

            // Confirmar la transacción.
            con.CommitTrans();

            // Devolver el resultado de la ejecución del comando.
            return await Task.FromResult(Unit.Value);
        }
        catch
        {
            // Fallo detectado. Deshacer transacción y relanzar la excepción.
            con.RollBackTrans();
            throw;
        }
    }

    #endregion    
}
