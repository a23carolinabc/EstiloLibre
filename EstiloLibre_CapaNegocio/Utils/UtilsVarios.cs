using Dapper;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace EstiloLibre_CapaNegocio.Utils
{
    public static class UtilsVarios
    {
        internal static string GenerarGuid()
        {
            return Guid.NewGuid().ToString();
        }

        internal static string GenerarHash(string strTexto)
        {
            byte[] aTextoEnBytes;
            byte[] aHash;
            string strHash;

            //Crea un array de bytes según el texto pasado
            aTextoEnBytes = Encoding.UTF8.GetBytes(strTexto);

            //Codifica según SHA256
            aHash = SHA256.HashData(aTextoEnBytes);
            strHash = Convert.ToHexString(aHash).ToLower();

            //Devolver el hash en formato hexadecimal.
            return strHash;
        }

        public static IEnumerable<T> MapearALista<T>(this DataTable tabla) where T : class
        {
            if (tabla == null || tabla.Rows.Count == 0)
            {
                return Enumerable.Empty<T>();
            }

            using (var reader = tabla.CreateDataReader())
            {
                var resultado = SqlMapper.Parse<T>(reader).ToList();
                return resultado;
            }
        }

        public static T? MapearAObjeto<T>(this DataTable tabla) where T : class
        {
            return tabla.MapearALista<T>().FirstOrDefault();
        }
    }
}
