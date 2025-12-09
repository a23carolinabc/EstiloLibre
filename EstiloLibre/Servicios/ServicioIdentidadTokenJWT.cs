using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using EstiloLibre_CapaNegocio.Servicios;

namespace EstiloLibre.Servicios
{
    public class ServicioIdentidadTokenJwt
    {
        private readonly IHttpContextAccessor _accesoContextoHTTP;
        private readonly ILogger _logger;
        private readonly ServicioAutentificacionPersonas _servicioAutentificacionPersonas;
        private readonly ServicioTokenPropio _srvTokenPropio;
        private DatosUsuarioEnClaims _datosUsuarioEnToken;

        #region ***** CONSTRUCTORES *****

        public ServicioIdentidadTokenJwt(IHttpContextAccessor accesoContextoHTTP,
                                         ILogger<ServicioIdentidadTokenJwt> logger,
                                         ServicioAutentificacionPersonas servicioAutentificacionPersonas,
                                         ServicioTokenPropio srvTokenPropio)
        {
            this._accesoContextoHTTP = accesoContextoHTTP;
            this._logger = logger;
            this._datosUsuarioEnToken = null;
            this._srvTokenPropio = srvTokenPropio;
            this._servicioAutentificacionPersonas = servicioAutentificacionPersonas;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public ResultadoAutentificacion AutentificarUsuarioEnCapaNegocio(CredencialesUsuario credenciales)
        {
            CredencialesUsuarioCapaNegocioDTO credencialesCapaNegocio;

            credencialesCapaNegocio = new CredencialesUsuarioCapaNegocioDTO()
            {
                NombreDeUsuario = credenciales.Login,
                Contraseña = credenciales.Contraseña,
                EstaAutenticado = this._accesoContextoHTTP.HttpContext?.User.Identity?.IsAuthenticated ?? false,
                DireccionCorreoElectronico = null,
                NumeroDocumentoIdentificacion = null
            };

            return this._servicioAutentificacionPersonas.Autentificar(credencialesCapaNegocio);
        }

        public string GetNombre()
        {
            return "TOKEN_JWT";
        }

        public DatosUsuarioEnClaims GetDatosUsuarioAutenticado()
        {
            DatosUsuarioEnClaims datosUsuario;

            //Si los datos ya fueron rescatados, devolverlos.
            if (this._datosUsuarioEnToken != null)
            {
                return this._datosUsuarioEnToken;
            }

            //Leer la información que ya está en el contexto (el token ya fue validado por .NET).
            datosUsuario = this._srvTokenPropio.LeerClaims(this._accesoContextoHTTP.HttpContext.User.Claims);

            //Devolver el resultado.
            return datosUsuario;
        }

        public string GenerarToken(UsuarioAutenticadoDTO usuarioAutenticado, bool bElUsuarioEsPersona)
        {
            return this._srvTokenPropio.GenerarToken(
                usuarioAutenticado,
                this._accesoContextoHTTP.HttpContext.Connection.RemoteIpAddress.ToString(),
                bElUsuarioEsPersona
            );
        }

        #endregion

    }
}
