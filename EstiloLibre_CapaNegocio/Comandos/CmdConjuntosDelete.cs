using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdConjuntosDelete : ComandoBase, IRequest<bool>
    {
        public int ConjuntoId { get; set; }

        public CmdConjuntosDelete(int iConjuntoId) : base([Codigos.Permisos.USER])
        {
            this.ConjuntoId = iConjuntoId;
        }
    }

    public class PcmdConjuntosDelete : ProcesadorDeComandoBase, IRequestHandler<CmdConjuntosDelete, bool>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdConjuntosDelete(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<bool> Handle(CmdConjuntosDelete comando, CancellationToken cancellationToken)
        {
            Conjunto? conjunto;
            Adjuntos adjuntos;
            PrendasConjuntos conjuntosPrendas;

            base.VericarPermisos(comando);

            try
            {
                // Envolver todo en una transacción
                this.con.BeginTrans();

                // Buscar el conjunto
                conjunto = this.con.CargarConjunto(comando.ConjuntoId);
                if (conjunto == null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Verificar que sea del usuario autenticado
                if (conjunto.UsuarioId != this.con.UsuarioAutenticado.Id)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_SinPermisos");
                }

                // Eliminar adjuntos (imágenes)
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }

                // Eliminar relaciones con prendas
                conjuntosPrendas = this.con.CargarPrendasConjuntos(conjunto.Id);
                foreach (PrendaConjunto cp in conjuntosPrendas)
                {
                    cp.Eliminar();
                }

                // Eliminar el conjunto
                conjunto.Eliminar();

                // Confirmar transacción
                this.con.CommitTrans();

                return await Task.FromResult(true);
            }
            catch
            {
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}