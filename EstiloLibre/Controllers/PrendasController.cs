using MediatR;
using Microsoft.AspNetCore.Mvc;
using EstiloLibre.Base;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasPrendas.Dtos;
using System.Net;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrendasController
    : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ServicioIdentidadTokenJwt _servicioIdentidad;
    private readonly ConsultasPrendas _consultasPrendas;

    #endregion

    #region ***** CONSTRUCTORES *****

    public PrendasController(IMediator mediator,
                              ServicioIdentidadTokenJwt servicioIdentidad,
                              ConsultasPrendas consultasPrendas)
    {
        this._mediador = mediator;
        this._servicioIdentidad = servicioIdentidad;
        this._consultasPrendas = consultasPrendas;
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****

    [Route("actualizarDatosSesion")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ActualizarDatosDeSesion([FromBody] CmdActualizarDatosSesion comando)
    {
        //Enviar el comando al mediador para su procesamiento.
        await _mediador.Send(comando);

        //Devolver el resultado de la ejecución.
        return Ok();
    }

    [Route("addnew")]
    [HttpGet]
    [ProducesResponseType(typeof(PrendasAddNewDto), (int)HttpStatusCode.OK)]
    public IActionResult AddNew()
    {
        PrendasAddNewDto objeto;

        //Recuperar datos necesarios para el addnew.
        objeto = this._consultasPrendas.GetDatosAddNew();

        //Devolver el resultado de la ejecución.
        return Ok(objeto);        
    }    
    #endregion
}
