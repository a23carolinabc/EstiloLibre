using System.Security.Cryptography;
using System.Text;

namespace EstiloLibre.Utils
{
    public static class UtilsVarios
    {        
        public static string GenerarHash(string strTexto)
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
    }

}
