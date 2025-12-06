using MediatR;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;

namespace EstiloLibre_CapaNegocio.Comandos
{
    /// <summary>
    /// Comando para guardar una prenda
    /// </summary>
    public class CmdPrendasSaveData : ComandoBase, IRequest<int>
    {
        public string FotoBase64 { get; set; }
        public int UsuarioId { get; set; }
        public int ColorId { get; set; }
        public int CategoriaId { get; set; }
        public int EstadoId { get; set; }
        public int TallaId { get; set; }
        public int MaterialId { get; set; }
        public int MarcaId { get; set; }
        public int EstacionId { get; set; }
        public decimal Precio { get; set; }
        public string EnlaceCompra { get; set; }
        public DateTime? FechaCompra { get; set; }

        public CmdPrendasSaveData() : base([])
        {
        }
    }

    /// <summary>
    /// Procesador del comando de guardar prenda
    /// </summary>
    public class PcmdPrendasSaveData : ProcesadorDeComandoBase, IRequestHandler<CmdPrendasSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamientoImagenes _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdPrendasSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamientoImagenes(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdPrendasSaveData comando, CancellationToken cancellationToken)
        {
            Prenda prenda;
            string nombreArchivoImagen;

            try
            {
                // Iniciar transacción
                con.BeginTrans();

                // 1. CREAR OBJETO PRENDA
                prenda = new Prenda()
                {
                    UsuarioId = comando.UsuarioId,
                    ColorId = comando.ColorId,
                    CategoriaId = comando.CategoriaId,
                    EstadoId = comando.EstadoId,
                    TallaId = comando.TallaId,
                    MaterialId = comando.MaterialId,
                    MarcaId = comando.MarcaId = comando.MarcaId,
                    EstacionId = comando.EstacionId = comando.EstacionId,
                    Precio = comando.Precio,
                    EnlaceCompra = comando.EnlaceCompra,
                    FechaCompra = comando.FechaCompra,
                    RutaFoto = string.Empty // Temporal, se actualiza después
                };

                // 2. GUARDAR PRENDA EN BD (para obtener el ID)
                prenda.Guardar();

                // 3. GUARDAR IMAGEN EN SISTEMA DE ARCHIVOS
                nombreArchivoImagen = await this._servicioAlmacenamiento.GuardarImagenPrenda(
                    comando.FotoBase64,
                    prenda.Id
                );

                // 4. ACTUALIZAR RUTA DE IMAGEN EN BD
                prenda.RutaFoto = nombreArchivoImagen;
                prenda.Guardar();

                // Confirmar transacción
                con.CommitTrans();

                // Devolver ID de la prenda creada
                return prenda.Id;
            }
            catch
            {
                // Revertir transacción en caso de error
                con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}