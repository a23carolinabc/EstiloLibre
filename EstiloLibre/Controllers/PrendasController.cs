using EstiloLibre.Base;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Comandos.CmdPrendasSaveData.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasConjuntos.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasPrendas.DTOs;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrendasController
    : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ConsultasPrendas _consultasPrendas;

    #endregion

    #region ***** CONSTRUCTORES *****

    public PrendasController(IMediator mediator,
                              ConsultasPrendas consultasPrendas)
    {
        this._mediador = mediator;
        this._consultasPrendas = consultasPrendas;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****
        

    [Route("addnew")]
    [HttpGet]
    [ProducesResponseType(typeof(PrendasAddNewDTO), (int)HttpStatusCode.OK)]
    public IActionResult AddNew()
    {
        PrendasAddNewDTO objeto;

        //Recuperar datos necesarios para el addnew.
        objeto = this._consultasPrendas.GetDatosAddNew();

        //Devolver el resultado de la ejecución.
        return Ok(objeto);        
    }

    [Route("showdata/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(PrendasShowDataDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ShowData([FromRoute] int id)
    {
        PrendasShowDataDTO objeto;

        //Recuperar datos necesarios para el showdata.
        objeto = await this._consultasPrendas.GetDatosShowData(id);

        //Devolver el resultado de la ejecución.
        return Ok(objeto);
    }

    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SaveData([FromBody] PrendaSaveDataDTO objeto)
    {
        CmdPrendasSaveData comando;
        int iPrendaId;

        comando = new CmdPrendasSaveData(objeto);
        iPrendaId = await this._mediador.Send(comando);
        return Ok(iPrendaId);
    }

    [Route("prendasUsuario/{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PrendaResumenDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPrendasUsuario([FromRoute] int id)
    {
        IEnumerable<PrendaResumenDTO> objeto;

        //Recuperar datos.
        objeto = await this._consultasPrendas.GetPrendasUsuario(id);

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
        CmdPrendasDelete comando;

        comando = new CmdPrendasDelete(id);
        await this._mediador.Send(comando);
        return Ok();
    }
    #endregion
}
