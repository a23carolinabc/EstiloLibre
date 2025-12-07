using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;
using static EstiloLibre_CapaNegocio.Comandos.CmdPrendasSaveData.Dtos;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdPrendasSaveData
        : ComandoBase, IRequest<int>
    {
        public PrendaSaveDataDTO Prenda { get; set; }
        public CmdPrendasSaveData(PrendaSaveDataDTO prendaSaveData) : base([Codigos.Permisos.USER])
        {
            this.Prenda = prendaSaveData;
        }
    }

    public class PcmdPrendasSaveData : ProcesadorDeComandoBase, IRequestHandler<CmdPrendasSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdPrendasSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdPrendasSaveData comando, CancellationToken cancellationToken)
        {
            Prenda? prenda;
            Adjunto adjunto;
            byte[] byImagen;
            bool bEsActualizacion;

            bEsActualizacion = false;
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
                    bEsActualizacion = true;
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

                //Asignamos el id del usuario autenticado.
                prenda.UsuarioId = this.con.UsuarioAutenticado.Id;

                //Guardar prenda para obtener id.
                prenda.Guardar();

                if (!string.IsNullOrEmpty(comando.Prenda.FotoBase64))
                {
                    if (bEsActualizacion)
                    {
                        Adjuntos adjuntosAntiguos = con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, prenda.Id);

                        foreach (Adjunto adjuntoAntiguo in adjuntosAntiguos)
                        {
                            // Eliminar archivo físico
                            this._servicioAlmacenamiento.EliminarArchivo(adjuntoAntiguo);

                            // Eliminar registro de BD
                            adjuntoAntiguo.Eliminar();
                        }
                    }

                    // Procesar imagen (redimensionar, convertir a WebP)
                    byImagen = await this._servicioAlmacenamiento.ProcesarImagen(comando.Prenda.FotoBase64);

                    // Crear nuevo adjunto en BD
                    adjunto = con.CrearAdjunto();
                    adjunto.ClaseObjetoId = Codigos.ClasesObjetos.Prenda;
                    adjunto.ObjetoId = prenda.Id;
                    adjunto.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjunto.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto.
                    adjunto.Guardar();

                    // Guardar archivo físico comprimido
                    await this._servicioAlmacenamiento.GuardarArchivo(adjunto, byImagen);
                }

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