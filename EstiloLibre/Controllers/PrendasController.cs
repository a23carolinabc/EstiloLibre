using EstiloLibre.Base;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasPrendas.Dtos;

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

    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SaveData([FromBody] JsonElement datosPrenda)
    {
        CmdPrendasSaveData comando;
        DatosUsuarioEnClaims datosUsuario;
        int prendaId;

        try
        {
            // Obtener usuario actual del token JWT
            datosUsuario = this._servicioIdentidad.GetDatosUsuarioAutenticado();

            // Crear comando con los datos recibidos
            comando = new CmdPrendasSaveData
            {
                UsuarioId = datosUsuario.UsuarioId,
                FotoBase64 = datosPrenda.GetProperty("fotoBase64").GetString(),
                ColorId = datosPrenda.GetProperty("colorId").GetInt32(),
                CategoriaId = datosPrenda.GetProperty("categoriaId").GetInt32(),
                EstadoId = datosPrenda.GetProperty("estadoId").GetInt32(),
                TallaId = datosPrenda.GetProperty("tallaId").GetInt32(),
                MaterialId = datosPrenda.GetProperty("materialId").GetInt32(),
                MarcaId = datosPrenda.TryGetProperty("marcaId", out var marcaId) ? marcaId.GetInt32() : 0,
                EstacionId = datosPrenda.TryGetProperty("estacionId", out var estacionId) ? estacionId.GetInt32() : 0,
                Precio = datosPrenda.TryGetProperty("precio", out var precio) ? precio.GetDecimal() : 0,
                EnlaceCompra = datosPrenda.TryGetProperty("enlaceCompra", out var enlace) ? enlace.GetString() : null,
                FechaCompra = datosPrenda.TryGetProperty("fechaCompra", out var fecha) && fecha.ValueKind != JsonValueKind.Null
                    ? DateTime.Parse(fecha.GetString())
                    : null
            };

            // Procesar comando
            prendaId = await this._mediador.Send(comando);

            return Ok(prendaId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error al guardar prenda: {ex.Message}" });
        }
    }
    #endregion
}
