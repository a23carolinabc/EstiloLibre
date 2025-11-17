using MediatR;
using Microsoft.AspNetCore.Mvc;
using EstiloLibre.Base;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using System.Net;

namespace EstiloLibre.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController
    : ControladorApiBase
{
    #region ***** PROPIEDADES *****

    private readonly IMediator _mediador;
    private readonly ServicioIdentidadTokenJwt _servicioIdentidad;
    private readonly ConsultasUsuarios _consultasUsuarios;

    #endregion

    #region ***** CONSTRUCTORES *****

    public UsuariosController(IMediator mediator,
                              ServicioIdentidadTokenJwt servicioIdentidad,
                              ConsultasUsuarios consultasUsuarios)
    {
        this._mediador = mediator;
        this._servicioIdentidad = servicioIdentidad;
        this._consultasUsuarios = consultasUsuarios;
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

    [Route("actualizarToken")]
    [HttpGet]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public IActionResult ActualizarToken()
    {
        DatosUsuarioEnClaims datosUsuarioAutenticado;
        UsuarioAutenticadoDTO datosUsuarioCapaNegocio;
        string strToken;

        //Recuperar los datos del usuario a partir de la petición actual.
        datosUsuarioAutenticado = this._servicioIdentidad.GetDatosUsuarioAutenticado();

        //Controlar el caso de que haya algún problema durante la identificación del usuario que hace
        //la petición.
        if (datosUsuarioAutenticado == null)
        {
            return BadRequest();
        }

        //Almacenar el resultado para no tener que volver a crearlo en sucesivas llamadas a este método.
        datosUsuarioCapaNegocio = new UsuarioAutenticadoDTO()
        {
            Id = datosUsuarioAutenticado.UsuarioId,
            Nombre = datosUsuarioAutenticado.NombrePersona,
            Apellidos = datosUsuarioAutenticado.Apellidos,
            Login = datosUsuarioAutenticado.Login,
            Permisos = new List<string>(datosUsuarioAutenticado.ListaPermisosAcceso),
            //IdiomaActualId = datosUsuarioAutenticado.IdiomaId,
            //CodigoIdiomaActual = datosUsuarioAutenticado.CodigoIdiomaActual
        };

        //Construir el token si las credenciales son correctas.
        strToken = this._servicioIdentidad.GenerarToken(datosUsuarioCapaNegocio, bElUsuarioEsPersona: true);
        
        //Devolver respuesta.
        return Ok(strToken);
    }

    [Route("savedata")]
    [HttpPost]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SaveData([FromBody] CmdUsuariosSaveData.Dtos.UsuarioSaveData Usuario)
    {
        CmdUsuariosSaveData comando;
        int resultado;

        comando = new CmdUsuariosSaveData(Usuario);
        resultado = await this._mediador.Send(comando);
        return Ok(resultado);
    }

    [Route("delete/{id}")]
    [HttpDelete]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        CmdUsuariosDelete comando;

        comando = new CmdUsuariosDelete(id);
        await this._mediador.Send(comando);
        return Ok();
    }
    #endregion
}
