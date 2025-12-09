using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;

namespace EstiloLibre_CapaNegocio.Servicios
{
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

            this._rutaBaseArchivos = rutaConfiguracion;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task GuardarArchivo(Adjunto adjunto, byte[] contenidoBytes)
        {
            string rutaCompleta;
            byte[] contenidoComprimido;

            try
            {
                // Validar parámetros
                if (adjunto == null)
                {
                    throw new CapaNegocioException("Adjunto nulo "+ nameof(adjunto));
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

        public void EliminarArchivo(Adjunto adjunto)
        {
            string rutaCompleta;

            try
            {
                if (adjunto == null)
                {
                    throw new CapaNegocioException("Adjunto nulo " + nameof(adjunto));
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
                throw new CapaNegocioException($"Error al eliminar archivo: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ObtenerContenidoArchivo(Adjunto adjunto)
        {
            string rutaCompleta;
            byte[] contenidoComprimido;
            byte[] contenidoDescomprimido;

            try
            {
                if (adjunto == null)
                {
                    throw new CapaNegocioException(nameof(adjunto));
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

        public string ObtenerRutaCompleta(Adjunto adjunto)
        {
            string rutaCompleta;

            if (adjunto == null)
            {
                throw new CapaNegocioException(nameof(adjunto));
            }

            if (string.IsNullOrEmpty(adjunto.Guid))
            {
                throw new CapaNegocioException("El adjunto no tiene guid asignado");
            }

            rutaCompleta = Path.Combine(this._rutaBaseArchivos, adjunto.Guid);
            return rutaCompleta;
        }

        public bool ExisteArchivo(Adjunto adjunto)
        {
            if (adjunto == null || string.IsNullOrEmpty(adjunto.Guid))
            {
                return false;
            }

            return File.Exists(this.ObtenerRutaCompleta(adjunto));
        }
                    
        public async Task<byte[]> ProcesarImagen(string imagenBase64)
        {
            byte[] imagenBytes;
            byte[] imagenProcesada;

            try
            {
                //if (imagenBase64.Contains(","))
                //{
                //    // Formato: "data:image/webp;base64,UklGRt..."
                //    // Tomar solo la parte después de la coma
                //    imagenBase64 = imagenBase64.Substring(imagenBase64.IndexOf(",") + 1);
                //}
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

        public async Task<byte[]> ProcesarImagenBytes(byte[] imagenBytes)
        {
            Image imagen;
            WebpEncoder encoder;
            byte[] imagenProcesada;

            try
            {
                using (imagen = Image.Load(imagenBytes))
                {
                    // Máximo 1200 píxeles de ancho para optimizar almacenamiento
                    if (imagen.Width > 1200)
                    {
                        imagen.Mutate(x => x.Resize(1200, 0)); // 0 == mantener proporción
                    }

                    // Compresos webp
                    encoder = new WebpEncoder
                    {
                        Quality = 85,
                        FileFormat = WebpFileFormatType.Lossy
                    };

                    // Guardar imagen en memoria
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