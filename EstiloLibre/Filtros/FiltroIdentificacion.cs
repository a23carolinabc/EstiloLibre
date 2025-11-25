using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;

namespace EstiloLibre.Filtros
{
    public class FiltroIdentificacion : IResourceFilter
    {
        #region ***** PROPIEDADES *****

        private readonly Conexion _con;
        private readonly ServicioIdentidadTokenJwt _servicioIdentidad;

        #endregion

        #region ***** CONSTRUCTORES *****

        public FiltroIdentificacion(Conexion con, ServicioIdentidadTokenJwt servicioIdentidad)
        {
            this._con = con;
            this._servicioIdentidad = servicioIdentidad;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            DatosUsuarioEnClaims datosUsuarioAutenticado;
            UsuarioAutenticadoDTO datosUsuarioCapaNegocio;

            //Recuperar los datos del usuario a partir de la petición actual.
            datosUsuarioAutenticado = this._servicioIdentidad.GetDatosUsuarioAutenticado();

            //Controlar el caso de que haya algún problema durante la identificación del usuario que hace
            //la petición.
            if (datosUsuarioAutenticado == null)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                context.Result = new UnauthorizedResult();
                return;
            }

            //Almacenar el resultado para no tener que volver a crearlo en sucesivas llamadas a este método.
            datosUsuarioCapaNegocio = new UsuarioAutenticadoDTO()
            {
                Id = datosUsuarioAutenticado.UsuarioId,
                Nombre = datosUsuarioAutenticado.NombrePersona,
                Apellidos = datosUsuarioAutenticado.Apellidos,
                Login = datosUsuarioAutenticado.Login,
                Permisos = new List<string>(datosUsuarioAutenticado.ListaPermisosAcceso),
                IdiomaActualId = datosUsuarioAutenticado.IdiomaId,
                CodigoIdiomaActual = datosUsuarioAutenticado.CodigoIdiomaActual
            };

            //Establecer el usuario en el objeto conexión.
            this._con.EstablecerUsuarioActual(datosUsuarioCapaNegocio);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //Sin acciones que hacer al ejecutar el recurso
        }

        #endregion
    }

    internal class NoAutorizado : ActionResult
    {
        public override void ExecuteResult(ActionContext contexto)
        {
            contexto.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
