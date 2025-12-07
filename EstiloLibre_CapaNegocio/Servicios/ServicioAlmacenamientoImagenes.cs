using EstiloLibre_CapaNegocio.Excepciones;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace EstiloLibre_CapaNegocio.Servicios
{
    public class ServicioAlmacenamientoImagenes
    {
        #region ***** PROPIEDADES *****

        private readonly string _rutaBaseImagenes;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioAlmacenamientoImagenes(Configuracion.Configuracion configuracion)
        {
            string rutaConfiguracion;

            // Leer ruta desde configuración
            rutaConfiguracion = configuracion.RutaGestorArchivos;

            // Si no existe en configuración
            if (string.IsNullOrEmpty(rutaConfiguracion) || !Directory.Exists(this._rutaBaseImagenes))
            {
                throw new CapaNegocioException("Ruta gestor de archivos no encontrada");
            }
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<string> GuardarImagenPrenda(string imagenBase64, int prendaId)
        {
            byte[] imagenBytes;
            string nombreArchivo;
            string rutaCompleta;
            Image imagen;
            WebpEncoder encoder;

            try
            {
                // 1. CONVERTIR BASE64 A BYTES
                imagenBytes = Convert.FromBase64String(imagenBase64);

                // 2. GENERAR NOMBRE ÚNICO PARA EL ARCHIVO
                // Formato: {prendaId}_{timestamp}.webp
                nombreArchivo = $"{prendaId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.webp";
                rutaCompleta = Path.Combine(this._rutaBaseImagenes, nombreArchivo);

                // 3. CARGAR IMAGEN EN MEMORIA
                using (imagen = Image.Load(imagenBytes))
                {
                    // 4. REDIMENSIONAR SI ES MUY GRANDE
                    // Máximo 1200 píxeles de ancho para optimizar almacenamiento
                    if (imagen.Width > 1200)
                    {
                        imagen.Mutate(x => x.Resize(1200, 0)); // 0 = mantener proporción
                    }

                    // 5. CONFIGURAR COMPRESOR WEBP
                    encoder = new WebpEncoder
                    {
                        Quality = 85, // Calidad 85/100 (buen balance tamaño/calidad)
                        FileFormat = WebpFileFormatType.Lossy // Compresión con pérdida
                    };

                    // 6. GUARDAR ARCHIVO
                    await imagen.SaveAsync(rutaCompleta, encoder);
                }

                // 7. DEVOLVER NOMBRE DEL ARCHIVO
                return nombreArchivo;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al guardar imagen: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina una imagen de prenda del sistema de archivos
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a eliminar</param>
        public void EliminarImagenPrenda(string nombreArchivo)
        {
            string rutaCompleta;

            try
            {
                // Validar que se proporcionó un nombre
                if (string.IsNullOrEmpty(nombreArchivo))
                    return;

                // Construir ruta completa
                rutaCompleta = Path.Combine(this._rutaBaseImagenes, nombreArchivo);

                // Eliminar si existe
                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                }
            }
            catch (Exception ex)
            {
                // No lanzar excepción, solo registrar error
                // (la eliminación de imágenes no debe bloquear otras operaciones)
                Console.WriteLine($"Error al eliminar imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene la ruta completa de una imagen
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <returns>Ruta completa del archivo</returns>
        public string ObtenerRutaCompleta(string nombreArchivo)
        {
            return Path.Combine(this._rutaBaseImagenes, nombreArchivo);
        }

        #endregion
    }
}