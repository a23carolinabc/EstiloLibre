using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;
using static EstiloLibre_CapaNegocio.Comandos.CmdPrendasSaveData.Dtos;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdPrendasSaveData
        : ComandoBase, IRequest<int>
    {
        public PrendaSaveDataDTO Prenda { get; set; }
        public CmdPrendasSaveData(PrendaSaveDataDTO prendaSaveData) : base([AccesoBD.Codigos.Permisos.MOD_Prendas])
        {
            this.Prenda = prendaSaveData;
        }
    }

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
            Prenda? prenda;
            string nombreArchivoImagen;

            base.VericarPermisos(comando);

            try
            {
                //Envolver todo el proceso en una transacción.
                con.BeginTrans();

                //Buscar si el objeto ya estaba registrado en BD.
                if (comando.Prenda.Id > 0)
                {
                    prenda = con.CargarPrenda(comando.Prenda.Id);
                    if (prenda == null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                    }
                }
                else
                {
                    prenda = con.CrearPrenda();
                }

                //Transferir propiedades del DTO al objeto de BD.
                prenda.ColorId = comando.Prenda.ColorId;
                prenda.CategoriaId = comando.Prenda.CategoriaId;
                prenda.EstadoId = comando.Prenda.EstadoId;
                prenda.TallaId = comando.Prenda.TallaId;
                prenda.MaterialId = comando.Prenda.MaterialId;
                prenda.MarcaId = comando.Prenda.MarcaId;
                prenda.EstacionId = comando.Prenda.EstacionId;
                prenda.Precio = comando.Prenda.Precio;
                prenda.EnlaceCompra = comando.Prenda.EnlaceCompra;
                prenda.FechaCompra = comando.Prenda.FechaCompra;
                prenda.RutaFoto = string.Empty;

                //Asignamos el id del usuario autenticado.
                prenda.UsuarioId = this.con.UsuarioAutenticado.Id;

                //Guardar prenda para obtener id.
                prenda.Guardar();

                // Guardar imagen en sistema de archivos.
                nombreArchivoImagen = await this._servicioAlmacenamiento.GuardarImagenPrenda(
                    comando.Prenda.FotoBase64,
                    prenda.Id
                );

                //Actualizar ruta de la foto.
                prenda.RutaFoto = nombreArchivoImagen;
                prenda.Guardar();

                // Confirmar transacción.
                con.CommitTrans();

                // Devolver id del objeto.
                return prenda.Id;
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