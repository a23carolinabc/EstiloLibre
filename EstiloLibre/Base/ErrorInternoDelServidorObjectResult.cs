using Microsoft.AspNetCore.Mvc;

namespace EstiloLibre.Base
{
    public class ErrorInternoDelServidorObjectResult : ObjectResult
    {
        public ErrorInternoDelServidorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
