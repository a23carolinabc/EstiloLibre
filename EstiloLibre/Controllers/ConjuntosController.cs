using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.Objetos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Comandos.CmdConjuntosSaveData.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasConjuntos.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConjuntosController
    : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasConjuntos _consultasConjuntos;

    #endregion

    #region ***** CONSTRUCTORES *****

    public ConjuntosController(IMediator mediator,
                              ConsultasConjuntos consultasConjuntos)
    {
        this._mediador = mediator;
        this._consultasConjuntos = consultasConjuntos;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****
     

    [Route("addnew")]
    [HttpGet]
    [ProducesResponseType(typeof(ConjuntosAddNewDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddNew()
    {
        ConjuntosAddNewDTO objeto;

        //Recuperar datos necesarios para el addnew.
        objeto = await this._consultasConjuntos.GetDatosAddNew();

        //Devolver el resultado de la ejecución.
        return Ok(objeto);        
    }

    [Route("showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(ConjuntosShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ShowData([FromRoute] int id)
    {
        ConjuntosShowDataDTO objeto;

        //Recuperar datos necesarios para el showdata.
        objeto = await this._consultasConjuntos.GetDatosShowData(id);

        //Devolver el resultado de la ejecución.
        return Ok(objeto);
    }

    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SaveData([FromBody] ConjuntoSaveDataDTO objeto)
    {
        CmdConjuntosSaveData comando;
        int iConjuntoId;

        comando = new CmdConjuntosSaveData(objeto);
        iConjuntoId = await this._mediador.Send(comando);
        return Ok(iConjuntoId);
    }

    [Route("conjuntosUsuario/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConjuntoResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetConjuntosUsuario([FromRoute] int id)
    {
        IEnumerable<ConjuntoResumenDTO> objeto;

        //Recuperar datos necesarios para el showdata.
        objeto = await this._consultasConjuntos.GetConjuntosUsuario(id);

        //Devolver el resultado de la ejecución.
        return Ok(objeto);
    }

    [Route("delete/{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        CmdConjuntosDelete comando;

        comando = new CmdConjuntosDelete(id);
        await this._mediador.Send(comando);
        return Ok();
    }
    #endregion
}
