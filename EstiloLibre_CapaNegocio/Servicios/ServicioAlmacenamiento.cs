using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;

namespace EstiloLibre_CapaNegocio.Servicios
{
    /// <summary>
    /// Servicio para gestionar el almacenamiento físico de archivos (guardar, eliminar, comprimir, descomprimir)
    /// NO gestiona objetos Adjunto en BD - eso se hace directamente en los comandos
    /// </summary>
    public class ServicioAlmacenamiento
    {
        #region ***** PROPIEDADES *****

        private readonly string _rutaBaseArchivos;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioAlmacenamiento(Configuracion.Configuracion configuracion)
        {
            string rutaConfiguracion;

            // Leer ruta desde configuración
            rutaConfiguracion = configuracion.RutaGestorArchivos;

            // Validar que existe la ruta
            if (string.IsNullOrEmpty(rutaConfiguracion))
            {
                throw new CapaNegocioException("RutaGestorArchivos no está configurada");
            }

            // Crear directorio si no existe
            if (!Directory.Exists(rutaConfiguracion))
            {
                try
                {
                    Directory.CreateDirectory(rutaConfiguracion);
                }
                catch (Exception ex)
                {
                    throw new CapaNegocioException($"No se pudo crear el directorio de archivos: {ex.Message}");
                }
            }

            this._rutaBaseArchivos = rutaConfiguracion;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS - OPERACIONES CON ARCHIVOS *****

        /// <summary>
        /// Guarda un archivo en el sistema de archivos (comprimido con GZip)
        /// </summary>
        /// <param name="adjunto">Objeto adjunto con información del archivo (debe tener NombreArchivo y ExtensionArchivo)</param>
        /// <param name="contenidoBytes">Contenido del archivo sin comprimir</param>
        public async Task GuardarArchivo(Adjunto adjunto, byte[] contenidoBytes)
        {
            string rutaCompleta;
            byte[] contenidoComprimido;

            try
            {
                // Validar parámetros
                if (adjunto == null)
                {
                    throw new ArgumentNullException(nameof(adjunto));
                }

                if (string.IsNullOrEmpty(adjunto.Guid))
                {
                    throw new CapaNegocioException("El adjunto debe tener guid asignado");
                }

                if (contenidoBytes == null || contenidoBytes.Length == 0)
                {
                    throw new CapaNegocioException("El contenido del archivo está vacío");
                }

                // Comprimir contenido
                contenidoComprimido = this.ComprimirArchivo(contenidoBytes);

                // Obtener ruta completa
                rutaCompleta = this.ObtenerRutaCompleta(adjunto);

                // Guardar archivo físico comprimido
                await File.WriteAllBytesAsync(rutaCompleta, contenidoComprimido);
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al guardar archivo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina un archivo del sistema de archivos
        /// </summary>
        /// <param name="adjunto">Objeto adjunto con información del archivo</param>
        public void EliminarArchivo(Adjunto adjunto)
        {
            string rutaCompleta;

            try
            {
                if (adjunto == null)
                {
                    return;
                }

                // Obtener ruta completa
                rutaCompleta = this.ObtenerRutaCompleta(adjunto);

                // Eliminar si existe
                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                }
            }
            catch (Exception ex)
            {
                // No lanzar excepción en eliminación para no bloquear operaciones
                // Solo registrar error
                Console.WriteLine($"Error al eliminar archivo: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el contenido descomprimido de un archivo
        /// </summary>
        /// <param name="adjunto">Objeto adjunto con información del archivo</param>
        /// <returns>Contenido del archivo descomprimido</returns>
        public async Task<byte[]> ObtenerContenidoArchivo(Adjunto adjunto)
        {
            string rutaCompleta;
            byte[] contenidoComprimido;
            byte[] contenidoDescomprimido;

            try
            {
                if (adjunto == null)
                {
                    throw new ArgumentNullException(nameof(adjunto));
                }

                // Obtener ruta del archivo
                rutaCompleta = this.ObtenerRutaCompleta(adjunto);

                if (!File.Exists(rutaCompleta))
                {
                    throw new CapaNegocioException($"No se encontró el archivo físico: {rutaCompleta}");
                }

                // Leer archivo comprimido
                contenidoComprimido = await File.ReadAllBytesAsync(rutaCompleta);

                // Descomprimir
                contenidoDescomprimido = this.DescomprimirArchivo(contenidoComprimido);

                return contenidoDescomprimido;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al obtener contenido del archivo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene la ruta completa del archivo físico
        /// </summary>
        /// <param name="adjunto">Objeto adjunto con información del archivo</param>
        /// <returns>Ruta completa del archivo</returns>
        public string ObtenerRutaCompleta(Adjunto adjunto)
        {
            string rutaCompleta;

            if (adjunto == null)
            {
                throw new ArgumentNullException(nameof(adjunto));
            }

            if (string.IsNullOrEmpty(adjunto.Guid))
            {
                throw new CapaNegocioException("El adjunto no tiene guid asignado");
            }

            rutaCompleta = Path.Combine(this._rutaBaseArchivos, adjunto.Guid);
            return rutaCompleta;
        }

        /// <summary>
        /// Verifica si existe el archivo físico
        /// </summary>
        /// <param name="adjunto">Objeto adjunto con información del archivo</param>
        /// <returns>True si el archivo existe</returns>
        public bool ExisteArchivo(Adjunto adjunto)
        {
            string rutaCompleta;

            try
            {
                if (adjunto == null)
                {
                    return false;
                }

                rutaCompleta = this.ObtenerRutaCompleta(adjunto);
                return File.Exists(rutaCompleta);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS - PROCESAMIENTO DE IMÁGENES *****

        /// <summary>
        /// Procesa una imagen: redimensiona si es necesario y convierte a formato WebP
        /// </summary>
        /// <param name="imagenBase64">Imagen en formato Base64</param>
        /// <returns>Bytes de la imagen procesada en formato WebP</returns>
        public async Task<byte[]> ProcesarImagen(string imagenBase64)
        {
            byte[] imagenBytes;
            byte[] imagenProcesada;

            try
            {
                // Convertir Base64 a bytes
                imagenBytes = Convert.FromBase64String(imagenBase64);

                // Procesar imagen
                imagenProcesada = await this.ProcesarImagenBytes(imagenBytes);

                return imagenProcesada;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al procesar imagen desde Base64: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Procesa una imagen: redimensiona si es necesario y convierte a formato WebP
        /// </summary>
        /// <param name="imagenBytes">Bytes de la imagen original</param>
        /// <returns>Bytes de la imagen procesada en formato WebP</returns>
        public async Task<byte[]> ProcesarImagenBytes(byte[] imagenBytes)
        {
            Image imagen;
            WebpEncoder encoder;
            byte[] imagenProcesada;

            try
            {
                using (imagen = Image.Load(imagenBytes))
                {
                    // REDIMENSIONAR SI ES MUY GRANDE
                    // Máximo 1200 píxeles de ancho para optimizar almacenamiento
                    if (imagen.Width > 1200)
                    {
                        imagen.Mutate(x => x.Resize(1200, 0)); // 0 = mantener proporción
                    }

                    // CONFIGURAR COMPRESOR WEBP
                    encoder = new WebpEncoder
                    {
                        Quality = 85, // Calidad 85/100 (buen balance tamaño/calidad)
                        FileFormat = WebpFileFormatType.Lossy // Compresión con pérdida
                    };

                    // GUARDAR EN MEMORIA
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await imagen.SaveAsync(stream, encoder);
                        imagenProcesada = stream.ToArray();
                    }
                }

                return imagenProcesada;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al procesar imagen: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene una imagen en formato Base64 con prefijo data:image/webp;base64,
        /// </summary>
        /// <param name="adjunto">Adjunto de la imagen</param>
        /// <returns>Imagen en formato Base64</returns>
        public async Task<string> ObtenerImagenBase64(Adjunto adjunto)
        {
            byte[] contenidoDescomprimido;
            string imagenBase64;

            try
            {
                // Obtener contenido descomprimido
                contenidoDescomprimido = await this.ObtenerContenidoArchivo(adjunto);

                // Convertir a Base64 con prefijo
                imagenBase64 = $"data:image/webp;base64,{Convert.ToBase64String(contenidoDescomprimido)}";

                return imagenBase64;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al obtener imagen en Base64: {ex.Message}", ex);
            }
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        /// <summary>
        /// Comprime un array de bytes usando GZip
        /// </summary>
        /// <param name="datos">Datos a comprimir</param>
        /// <returns>Datos comprimidos</returns>
        private byte[] ComprimirArchivo(byte[] datos)
        {
            byte[] datosComprimidos;

            try
            {
                using (MemoryStream streamSalida = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(streamSalida, CompressionMode.Compress))
                    {
                        gzip.Write(datos, 0, datos.Length);
                    }

                    datosComprimidos = streamSalida.ToArray();
                }

                return datosComprimidos;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al comprimir archivo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Descomprime un array de bytes usando GZip
        /// </summary>
        /// <param name="datosComprimidos">Datos comprimidos</param>
        /// <returns>Datos descomprimidos</returns>
        private byte[] DescomprimirArchivo(byte[] datosComprimidos)
        {
            byte[] datosDescomprimidos;

            try
            {
                using (MemoryStream streamEntrada = new MemoryStream(datosComprimidos))
                using (MemoryStream streamSalida = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(streamEntrada, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(streamSalida);
                    }

                    datosDescomprimidos = streamSalida.ToArray();
                }

                return datosDescomprimidos;
            }
            catch (Exception ex)
            {
                throw new CapaNegocioException($"Error al descomprimir archivo: {ex.Message}", ex);
            }
        }

        #endregion
    }
}