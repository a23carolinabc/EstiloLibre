using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using static EstiloLibre_CapaNegocio.Comandos.CmdConjuntosSaveData.DTOs;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdConjuntosSaveData : ComandoBase, IRequest<int>
    {
        public ConjuntoSaveDataDTO Conjunto { get; set; }

        public CmdConjuntosSaveData(ConjuntoSaveDataDTO conjuntoSaveData) : base([Codigos.Permisos.USER])
        {
            this.Conjunto = conjuntoSaveData;
        }
    }

    public class PcmdConjuntosSaveData : ProcesadorDeComandoBase, IRequestHandler<CmdConjuntosSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdConjuntosSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdConjuntosSaveData comando, CancellationToken cancellationToken)
        {
            Conjunto? conjunto;
            Adjunto adjunto;
            byte[] byImagen;
            bool bEsActualizacion;
            PrendasConjuntos conjuntosPrendasAntiguos;

            bEsActualizacion = false;
            base.VericarPermisos(comando);

            try
            {
                // Envolver todo el proceso en una transacción
                this.con.BeginTrans();

                // Buscar si el objeto ya estaba registrado en BD
                if (comando.Conjunto.Id > 0)
                {
                    conjunto = this.con.CargarConjunto(comando.Conjunto.Id);
                    if (conjunto == null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                    }
                    bEsActualizacion = true;
                }
                else
                {
                    conjunto = this.con.CrearConjunto();
                }

                // Transferir propiedades del DTO al objeto de BD
                conjunto.EstacionId = comando.Conjunto.EstacionId;
                conjunto.EstiloId = comando.Conjunto.EstiloId;
                conjunto.Descripcion = comando.Conjunto.Descripcion;
                conjunto.Etiquetas = comando.Conjunto.Etiquetas;
                conjunto.EsFavorito = comando.Conjunto.EsFavorito;
                conjunto.DatosComposicion = comando.Conjunto.DatosComposicion;
                conjunto.NotasPersonales = comando.Conjunto.NotasPersonales;

                // Asignar el id del usuario autenticado
                conjunto.UsuarioId = this.con.UsuarioAutenticado.Id;

                // Guardar conjunto para obtener id
                conjunto.Guardar();

                // Gestionar relaciones con prendas
                if (bEsActualizacion)
                {
                    // Eliminar relaciones antiguas
                    conjuntosPrendasAntiguos = this.con.CargarPrendasConjuntos(conjunto.Id);
                    foreach (PrendaConjunto cpAntiguo in conjuntosPrendasAntiguos)
                    {
                        cpAntiguo.Eliminar();
                    }
                }

                // Crear nuevas relaciones
                foreach (int iPrendaId in comando.Conjunto.PrendasIds)
                {
                    PrendaConjunto conjuntoPrenda;

                    conjuntoPrenda = this.con.CrearPrendaConjunto();
                    conjuntoPrenda.ConjuntoId = conjunto.Id;
                    conjuntoPrenda.PrendaId = iPrendaId;
                    conjuntoPrenda.Guardar();
                }

                // Gestionar imagen compuesta
                if (!string.IsNullOrEmpty(comando.Conjunto.ImagenBase64))
                {
                    if (bEsActualizacion)
                    {
                        Adjuntos adjuntosAntiguos;

                        adjuntosAntiguos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);

                        foreach (Adjunto adjuntoAntiguo in adjuntosAntiguos)
                        {
                            // Eliminar archivo físico
                            this._servicioAlmacenamiento.EliminarArchivo(adjuntoAntiguo);

                            // Eliminar registro de BD
                            adjuntoAntiguo.Eliminar();
                        }
                    }

                    // Procesar imagen (redimensionar, convertir a WebP)
                    byImagen = await this._servicioAlmacenamiento.ProcesarImagen(comando.Conjunto.ImagenBase64);

                    // Crear nuevo adjunto en BD
                    adjunto = this.con.CrearAdjunto();
                    adjunto.ClaseObjetoId = Codigos.ClasesObjetos.Conjunto;
                    adjunto.ObjetoId = conjunto.Id;
                    adjunto.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjunto.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto
                    adjunto.Guardar();

                    // Guardar archivo físico comprimido
                    await this._servicioAlmacenamiento.GuardarArchivo(adjunto, byImagen);
                }

                // Confirmar transacción
                this.con.CommitTrans();

                // Devolver id del objeto
                return conjunto.Id;
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