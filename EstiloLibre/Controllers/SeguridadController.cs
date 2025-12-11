using EstiloLibre.Filtros;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Comandos;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static EstiloLibre_CapaNegocio.Comandos.CmdNuevoUsuarioSaveData.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuarios.DTOs;

namespace EstiloLibre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    [ServiceFilter(typeof(FiltroAbrirCerrarConexion))]
    public class SeguridadController : ControllerBase
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioIdentidadTokenJwt _servicioIdentidad;
        private readonly ServicioCredencialesCabecera _servicioCredencialesCabecera;
        private readonly ConsultasUsuarios _consultasUsuarios;
        private readonly Conexion _con;
        private readonly IMediator _mediador;

        #endregion

        #region ***** CONSTRUCTORES *****

        public SeguridadController(ServicioIdentidadTokenJwt servicioIdentidad,
                                   IConfiguration configuration,                                   
                                   Conexion conexion,
                                   IMediator mediator,
                                   ConsultasUsuarios consultasUsuarios,
                                    ServicioCredencialesCabecera servicioCredencialesCabecera)
        {
            this._mediador = mediator;
            this._servicioIdentidad = servicioIdentidad;
            this._con = conexion;
            this._consultasUsuarios = consultasUsuarios;
            this._servicioCredencialesCabecera = servicioCredencialesCabecera;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
        [Route("autenticar")]
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult Autentificar()
        {
            string strToken;
            CredencialesUsuario credenciales;
            ResultadoAutentificacion resultadoAutenticacion;
            DatosSesionDTO datosDeSesion;

            //Obtenemos la cabecera Authorization
            string? authHeader = this.HttpContext.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized("");
            }

            //Decodificamos la cabecera
            credenciales = this._servicioCredencialesCabecera.ObtenerCredenciales(authHeader);

            //Comprobar credenciales de acceso en capa negocio.
            resultadoAutenticacion = this._servicioIdentidad.AutentificarUsuarioEnCapaNegocio(credenciales);

            //Si el usuario no existe en la BD o este está deshabilitado, devolver un error.
            if (!resultadoAutenticacion.Correcto)
            {
                return Unauthorized("");
            }
            //Construir el token si las credenciales son correctas.
            strToken = this._servicioIdentidad.GenerarToken(resultadoAutenticacion.UsuarioAutenticado, bElUsuarioEsPersona: true);

            //Cargar los datos de sesión.
            datosDeSesion = this._con.GetDatosDeSesion(resultadoAutenticacion.UsuarioAutenticado);

            //Devolver respuesta.
            return Ok(new { Token = strToken, DatosDeSesion = datosDeSesion });
        }        

        [Route("getDatosSesion")]
        [HttpGet]
        [ProducesResponseType(typeof(DatosSesionDTO), (int)HttpStatusCode.OK)]
        [ServiceFilter(typeof(FiltroAbrirCerrarConexion))]
        [ServiceFilter(typeof(FiltroIdentificacion))]
        public ActionResult<DatosSesionDTO> GetDatosDeSesion()
        {
            DatosSesionDTO datosDeSesion;

            //Cargar los datos de sesión.
            datosDeSesion = this._con.GetDatosDeSesion(this._con.UsuarioAutenticado);

            //Devolver el resultado de la ejecución.
            return Ok(datosDeSesion);
        }

        [Route("addnew")]
        [HttpGet]
        [ProducesResponseType(typeof(UsuarioAddNewDTO), (int)HttpStatusCode.OK)]
        public IActionResult AddNew()
        {
            UsuarioAddNewDTO objeto;

            //Recuperar datos necesarios para el addnew.
            objeto = this._consultasUsuarios.GetDatosAddNew();

            //Devolver el resultado de la ejecución.
            return Ok(objeto);
        }

        [Route("savedata")]
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> SaveData([FromBody] NuevoUsuarioSaveDataDTO Usuario)
        {
            CmdNuevoUsuarioSaveData comando;
            int resultado;

            comando = new CmdNuevoUsuarioSaveData(Usuario);
            resultado = await this._mediador.Send(comando);
            return Ok(resultado);
        }

        #endregion
    }
}
