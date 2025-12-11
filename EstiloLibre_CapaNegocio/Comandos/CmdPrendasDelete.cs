using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdPrendasDelete : ComandoBase, IRequest<bool>
    {
        public int PrendaId { get; set; }

        public CmdPrendasDelete(int iPrendaId) : base([Codigos.Permisos.USER])
        {
            this.PrendaId = iPrendaId;
        }
    }

    public class PcmdPrendasDelete : ProcesadorDeComandoBase, IRequestHandler<CmdPrendasDelete, bool>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdPrendasDelete(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<bool> Handle(CmdPrendasDelete comando, CancellationToken cancellationToken)
        {
            Prenda? prenda;
            Adjuntos adjuntos;
            PrendasConjuntos conjuntosPrendas;

            base.VericarPermisos(comando);

            try
            {
                // Envolver todo en una transacción
                this.con.BeginTrans();

                // Buscar el conjunto
                prenda = this.con.CargarPrenda(comando.PrendaId);
                if (prenda == null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Verificar que sea del usuario autenticado
                if (prenda.UsuarioId != this.con.UsuarioAutenticado.Id)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_SinPermisos");
                }

                // Verificar que no haya conjuntos que usen la prenda
                conjuntosPrendas = this.con.CargarPrendasConjuntosPorPrendas(prenda.Id);
                if(conjuntosPrendas != null && conjuntosPrendas.Any())
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_PrendaEnConjuntos");
                }

                // Eliminar adjuntos
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prenda.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }               

                // Eliminar el conjunto
                prenda.Eliminar();

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