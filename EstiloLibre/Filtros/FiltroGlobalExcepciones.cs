using Microsoft.AspNetCore.Mvc.Filters;
using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using System.Net;

namespace EstiloLibre.Filtros
{
    public class FiltroGlobalExcepciones : IExceptionFilter
    {
        #region ***** PROPIEDADES *****

        private readonly IWebHostEnvironment _entornoHost;
        private readonly ILogger<FiltroGlobalExcepciones> _logger;

        #endregion

        #region ***** CONSTRUCTORES *****

        public FiltroGlobalExcepciones(
            IWebHostEnvironment entornoHost, ILogger<FiltroGlobalExcepciones> logger)
        {
            this._entornoHost = entornoHost;
            this._logger = logger;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void OnException(ExceptionContext context)
        {
            RespuestaError respuesta;

            if (context.Exception is ReglaNegocioParaUsuarioException excepcionParaUsuario)
            {
                //Registrar la excepción en informes de error sólo si se está en el entorno de desarrollo.
                if (this._entornoHost.IsDevelopment())
                {
                    this._logger.LogError(context.Exception, "");
                }

                if (excepcionParaUsuario.Codigo == "PermisosUsuario_RN_001")
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
                else
                {
                    //Crear un objeto de respuesta con un mensaje de error genérico.
                    respuesta = new RespuestaError(context.Exception.Message);

                    //Colocar el objeto resultado en la respuesta y enviar el código de error 500 al cliente.
                    context.Result = new ErrorInternoDelServidorObjectResult(respuesta);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                //Registrar la excepción en informes de error.
                this._logger.LogError(context.Exception, "");

                //Crear un objeto de respuesta con un mensaje de error genérico.
                respuesta = new RespuestaError("ERR_ErrorServidor");

                //Colocar el objeto resultado en la respuesta y enviar el código de error 500 al cliente.
                context.Result = new ErrorInternoDelServidorObjectResult(respuesta);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            //Marcar el error como controlado.            
            context.ExceptionHandled = true;
        }

        #endregion

        #region ***** CLASES INTERNAS *****

        private class RespuestaError
        {
            public string Mensaje { get; private set; }

            public RespuestaError(string strMensaje)
            {
                this.Mensaje = strMensaje;
            }
        }

        #endregion
    }
}
