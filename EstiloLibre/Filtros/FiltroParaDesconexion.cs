using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace EstiloLibre.Filtros
{
    public class FiltroParaDesconexion
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            StringValues valores;
            string strURLRedirect;

            //Anular la redirección y ubicar la URL de redirección en el body de la respuesta.
            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Redirect)
            {
                //Cancelar la redirección.
                context.HttpContext.Response.StatusCode = 200;

                //Rescatar la URL de redirección de la cabecera y añadirla al body de la respuesta.
                //De esta forma, el navegador no hará redirección por su cuenta.
                if (context.HttpContext.Response.Headers.TryGetValue("Location", out valores))
                {
                    strURLRedirect = valores[0];

                    //Enviar la URL en el body para que Angular la pueda rescatar y abrir como mejor le
                    //convenga y no que sea el navegador el que haga la redirección.
                    context.HttpContext.Response.WriteAsync(strURLRedirect);
                }
            }
        }
    }
}
