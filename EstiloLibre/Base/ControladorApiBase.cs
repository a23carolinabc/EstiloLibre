using Microsoft.AspNetCore.Mvc;
using EstiloLibre.Filtros;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace EstiloLibre.Base
{
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    //[Authorize]
    [ServiceFilter(typeof(FiltroAbrirCerrarConexion))]
    [ServiceFilter(typeof(FiltroIdentificacion))]
    [ServiceFilter(typeof(FiltroEstablecerDatosSesion))]
    public class ControladorApiBase : ControllerBase
    {
    }
}
