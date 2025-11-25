using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EstiloLibre.Configuraciones;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstiloLibre.Servicios
{
    public class ServicioTokenPropio
    {
        public const string CLAIM_LOGIN = "Login";
        public const string CLAIM_PERMISOS = "PermisosAcceso";
        public const string CLAIM_Id = "UsuarioId";
        public const string CLAIM_NOMBRE = "NombrePersona";
        public const string CLAIM_APELLIDOS = "Apellidos";
        public const string CLAIM_DIRECCION_IP = "DireccionIP";
        public const string CLAIM_ES_PERSONA = "EsPersona";

        private readonly ConfiguracionAutenticacion _configAutenticacion;

        #region ***** CONSTRUCTORES *****

        public ServicioTokenPropio(IOptionsSnapshot<ConfiguracionAutenticacion> configAutenticacion)
        {
            this._configAutenticacion = configAutenticacion.Value;
        }

        #endregion

        public ResultadoValidarToken ValidarToken(string strToken)
        {
            SecurityToken token;
            JwtSecurityTokenHandler validadorTokenJWT;
            TokenValidationParameters parametrosValidacion;
            SymmetricSecurityKey claveSimetrica;

            try
            {
                //Configuración de la validación.
                claveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configAutenticacion.TokenJWT_ClaveSecreta));
                parametrosValidacion = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = claveSimetrica,
                    ValidIssuer = this._configAutenticacion.EmisorToken,
                    ValidAudience = this._configAutenticacion.AudienciaToken,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false
                };

                //Lanzar la validación.
                validadorTokenJWT = new JwtSecurityTokenHandler();
                validadorTokenJWT.ValidateToken(strToken, parametrosValidacion, out token);

                //Devolver resultado correcto.
                return new ResultadoValidarToken((JwtSecurityToken)token);
            }
            catch (Exception excp)
            {
                //No ha superado la validación. Devolver error pero sin lanzar excepción para que luego se pueda
                //tratar como una respuesta 401.
                return new ResultadoValidarToken(excp.Message);
            }
        }

        public DatosUsuarioEnClaims LeerClaims(IEnumerable<Claim> enumClaims)
        {
            DatosUsuarioEnClaims resultado;

            //Instanciar el objeto resultado.
            resultado = new DatosUsuarioEnClaims();

            //Lectura secuencial de claims.
            foreach (Claim claim in enumClaims)
            {
                if (claim.Type == ServicioTokenPropio.CLAIM_PERMISOS)
                {
                    resultado.AñadirPermisoAcceso(claim.Value);
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_Id)
                {
                    resultado.UsuarioId = int.Parse(claim.Value);
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_LOGIN)
                {
                    resultado.Login = claim.Value;
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_NOMBRE)
                {
                    resultado.NombrePersona = claim.Value;
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_APELLIDOS)
                {
                    resultado.Apellidos = claim.Value;
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_DIRECCION_IP)
                {
                    resultado.DireccionIPLogin = claim.Value;
                }
                else if (claim.Type == ServicioTokenPropio.CLAIM_ES_PERSONA)
                {
                    resultado.EsPersona = bool.Parse(claim.Value);
                }
            }

            //Devolver el resultado.
            return resultado;
        }

        public string GenerarToken(UsuarioAutenticadoDTO usuarioAutenticado,
                                   string strDireccionIP,
                                   bool bElUsuarioEsPersona)
        {
            JwtHeader cabecera;
            SigningCredentials credencialesEncriptacion;
            SymmetricSecurityKey claveSimetrica;
            Claim[] aClaimsApp;
            JwtPayload payload;
            JwtSecurityToken token;
            DateTime dtFechaExpiracion;

            //Cabecera del token.
            claveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configAutenticacion.TokenJWT_ClaveSecreta));
            credencialesEncriptacion = new SigningCredentials(claveSimetrica, SecurityAlgorithms.HmacSha512);
            cabecera = new JwtHeader(credencialesEncriptacion);

            //Claims específicos de la aplicación. Los permisos no se pueden añadir como un objeto Claim
            //porque este objeto no soporta listas. Para ello hay que añadirlo posteriormente al objeto
            //JwtPayload que hereda de diccionario.
            aClaimsApp = new[]
            {
                new Claim(ServicioTokenPropio.CLAIM_Id, usuarioAutenticado.Id.ToString()),
                new Claim(ServicioTokenPropio.CLAIM_LOGIN, usuarioAutenticado.Login),
                new Claim(ServicioTokenPropio.CLAIM_NOMBRE, usuarioAutenticado.Nombre),
                new Claim(ServicioTokenPropio.CLAIM_APELLIDOS, usuarioAutenticado.Apellidos),
                new Claim(ServicioTokenPropio.CLAIM_DIRECCION_IP, strDireccionIP),
                new Claim(ServicioTokenPropio.CLAIM_ES_PERSONA, bElUsuarioEsPersona.ToString())
            };

            //Determinar la fecha de expiración del token.
            if (bElUsuarioEsPersona)
            {
                dtFechaExpiracion = DateTime.Now.AddMinutes(this._configAutenticacion.TokenJWT_DuracionTokenEnMinutos);
            }
            else
            {
                dtFechaExpiracion = DateTime.Now.AddSeconds(this._configAutenticacion.DuracionTokenEnSegundosParaServicios);
            }

            //PayLoad del token.
            payload = new JwtPayload(
               issuer: this._configAutenticacion.EmisorToken,
               audience: this._configAutenticacion.AudienciaToken,
               claims: aClaimsApp,
               notBefore: DateTime.Now,
               expires: dtFechaExpiracion
            );
            payload.Add(ServicioTokenPropio.CLAIM_PERMISOS, usuarioAutenticado.Permisos);

            //Construcción del token y firmado.
            //JsonExtensions.Serializer = JsonConvert.SerializeObject;
            //JsonExtensions.Deserializer = JsonConvert.DeserializeObject;

            token = new JwtSecurityToken(cabecera, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region ***** CLASES INTERNAS *****

        public class ResultadoValidarToken
        {
            private JwtSecurityToken _token;

            public bool Correcto { get; }
            public string MensajeError { get; }

            public ResultadoValidarToken(JwtSecurityToken token)
            {
                this.Correcto = true;
                this._token = token;
            }

            public ResultadoValidarToken(string strMensajeError)
            {
                this.Correcto = false;
                this.MensajeError = strMensajeError;
            }

            public IEnumerable<Claim> GetClaims()
            {
                return this._token.Claims;
            }
        }

        #endregion
    }
}
