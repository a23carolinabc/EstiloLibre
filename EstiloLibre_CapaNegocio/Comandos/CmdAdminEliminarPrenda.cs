using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    /// <summary>
    /// Comando para que un administrador elimine una prenda de un usuario
    /// </summary>
    public class CmdAdminEliminarPrenda : ComandoBase, IRequest
    {
        public int PrendaId { get; set; }

        public CmdAdminEliminarPrenda(int iPrendaId) : base()
        {
            this.PrendaId = iPrendaId;
        }
    }

    public class PcmdAdminEliminarPrenda : ProcesadorDeComandoBase, IRequestHandler<CmdAdminEliminarPrenda>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdAdminEliminarPrenda(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public Task Handle(CmdAdminEliminarPrenda comando, CancellationToken cancellationToken)
        {
            Prenda? prenda;
            Adjuntos adjuntos;
            PrendasConjuntos prendasConjuntos;

            try
            {
                this.con.BeginTrans();

                // Cargar la prenda
                prenda = this.con.CargarPrenda(comando.PrendaId);
                if (prenda is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Eliminar relaciones con conjuntos
                prendasConjuntos = this.con.CargarPrendasConjuntosPorPrenda(comando.PrendaId);
                foreach (PrendaConjunto prendaConjunto in prendasConjuntos)
                {
                    prendaConjunto.Eliminar();
                }

                // Eliminar adjuntos físicos de la prenda
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prenda.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }

                // Eliminar prenda
                prenda.Eliminar();

                // Confirmar la transacción
                this.con.CommitTrans();

                return Task.FromResult(Unit.Value);
            }
            catch
            {
                // Fallo detectado. Deshacer transacción y relanzar la excepción
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}

