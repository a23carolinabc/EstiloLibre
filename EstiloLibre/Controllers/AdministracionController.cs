using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasAdministracion.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdministracionController : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasAdministracion _consultasAdministracion;

    #endregion

    #region ***** CONSTRUCTORES *****

    public AdministracionController(IMediator mediator,
                                    ConsultasAdministracion consultasAdministracion)
    {
        this._mediador = mediator;
        this._consultasAdministracion = consultasAdministracion;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    /// <summary>
    /// Obtiene un listado de usuarios normales (sin permiso ADMIN) con búsqueda opcional
    /// </summary>
    [Route("usuariosNormales/listado")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioNormalResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetListadoUsuariosNormales([FromQuery] string? textoBusqueda, [FromQuery] string? tipoBusqueda)
    {
        IEnumerable<UsuarioNormalResumenDTO> lista;

        // Recuperar listado de usuarios normales
        lista = await this._consultasAdministracion.GetListadoUsuariosNormales(textoBusqueda, tipoBusqueda);

        // Devolver el resultado de la ejecución
        return Ok(lista);
    }

    /// <summary>
    /// Obtiene los datos completos de un usuario normal para vista de administración
    /// </summary>
    [Route("usuariosNormales/showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioNormalShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDatosUsuarioNormal([FromRoute] int id)
    {
        UsuarioNormalShowDataDTO objeto;

        // Recuperar datos del usuario normal con sus prendas y conjuntos
        objeto = await this._consultasAdministracion.GetDatosUsuarioNormalParaAdmin(id);

        // Devolver el resultado de la ejecución
        return Ok(objeto);
    }

    /// <summary>
    /// Elimina una prenda de un usuario normal (vista de administración)
    /// </summary>
    [Route("usuariosNormales/prenda/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> EliminarPrenda([FromRoute] int id)
    {
        CmdAdminEliminarPrenda comando;

        comando = new CmdAdminEliminarPrenda(id);
        await this._mediador.Send(comando);

        return Ok();
    }

    /// <summary>
    /// Elimina un conjunto de un usuario normal (vista de administración)
    /// </summary>
    [Route("usuariosNormales/conjunto/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> EliminarConjunto([FromRoute] int id)
    {
        CmdAdminEliminarConjunto comando;

        comando = new CmdAdminEliminarConjunto(id);
        await this._mediador.Send(comando);

        return Ok();
    }

    #endregion
}
