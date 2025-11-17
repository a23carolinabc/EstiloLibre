using EstiloLibre.Utils;
using EstiloLibre_CapaNegocio.AccesoBD;
using System.Text;

namespace EstiloLibre.Servicios
{
    public class ServicioCredencialesCabecera
    {
        public CredencialesUsuario ObtenerCredenciales(string authHeader)
        {
            string encodedAuthHeader = authHeader.Substring("Basic ".Length).Trim();
            //Descodificamos las credenciales
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string decodedAuthHeader = encoding.GetString(Convert.FromBase64String(encodedAuthHeader));

            //Separamos los datos
            string login = decodedAuthHeader.Substring(0, decodedAuthHeader.IndexOf(':'));
            string contraseña = UtilsVarios.GenerarHash(decodedAuthHeader.Substring(decodedAuthHeader.IndexOf(':') + 1));

            CredencialesUsuario credenciales = new CredencialesUsuario()
            {
                Login = login,
                Contraseña = contraseña
            };
            return credenciales;
        }
    }
}
