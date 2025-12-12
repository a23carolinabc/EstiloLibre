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
    /// Comando para que un administrador elimine un conjunto de un usuario
    /// </summary>
    public class CmdAdminEliminarConjunto : ComandoBase, IRequest
    {
        public int ConjuntoId { get; set; }

        public CmdAdminEliminarConjunto(int iConjuntoId) : base()
        {
            this.ConjuntoId = iConjuntoId;
        }
    }

    public class PcmdAdminEliminarConjunto : ProcesadorDeComandoBase, IRequestHandler<CmdAdminEliminarConjunto>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdAdminEliminarConjunto(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public Task Handle(CmdAdminEliminarConjunto comando, CancellationToken cancellationToken)
        {
            Conjunto? conjunto;
            Adjuntos adjuntos;
            PrendasConjuntos prendasConjuntos;

            try
            {
                this.con.BeginTrans();

                // Cargar el conjunto
                conjunto = this.con.CargarConjunto(comando.ConjuntoId);
                if (conjunto is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Eliminar relaciones con prendas
                prendasConjuntos = this.con.CargarPrendasConjuntosPorConjunto(comando.ConjuntoId);
                foreach (PrendaConjunto prendaConjunto in prendasConjuntos)
                {
                    prendaConjunto.Eliminar();
                }

                // Eliminar adjuntos físicos del conjunto
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }

                // Eliminar conjunto
                conjunto.Eliminar();

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

