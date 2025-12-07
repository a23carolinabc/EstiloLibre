using Microsoft.AspNetCore.Mvc.Filters;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using System.Globalization;

namespace EstiloLibre.Filtros
{
    public class FiltroEstablecerDatosSesion : IResourceFilter
    {
        #region ***** PROPIEDADES *****

        private readonly Conexion _con;

        #endregion

        #region ***** CONSTRUCTORES *****

        public FiltroEstablecerDatosSesion(Conexion con)
        {
            this._con = con;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            IHeaderDictionary cabecerasHTTP;
            string strCodigoIdioma;

            cabecerasHTTP = context.HttpContext.Request.Headers;

            //Idioma.
            strCodigoIdioma = "gl-ES";
            if (cabecerasHTTP.ContainsKey("X-EstiloLibre-CodigoIdioma"))
            {
                strCodigoIdioma = cabecerasHTTP["X-EstiloLibre-CodigoIdioma"];
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(strCodigoIdioma);
            }

            //Establecer datos de sesión.
            this._con.EstablecerDatosSesion(new DatosSesionDTO()
            {
                CodigoIdioma = strCodigoIdioma
            });
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //No hacer nada.
        }

        #endregion
    }
}
