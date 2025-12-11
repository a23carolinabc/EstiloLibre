using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;

namespace EstiloLibre_CapaNegocio.Servicios
{
    public class ServicioAutentificacionPersonas
    {
        #region ***** PROPIEDADES INTERNAS *****

        private readonly Conexion _con;
        private readonly ConsultasUsuarios _consultasUsuarios;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ServicioAutentificacionPersonas(Conexion con, ConsultasUsuarios consultasUsuarios)
        {
            this._con = con;
            this._consultasUsuarios = consultasUsuarios;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public ResultadoAutentificacion Autentificar(CredencialesUsuarioCapaNegocioDTO datosUsuario)
        {
            ResultadoAutentificacion resultado;

            resultado = this.AutentificarUsuario(datosUsuario);
            if (!resultado.Correcto)
            {
                return new ResultadoAutentificacion(ResultadoAutentificacion.CodigosErrorAutentificacion.ERR_NoEsUsuarioApp);
            }
            return resultado;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private ResultadoAutentificacion AutentificarUsuario(CredencialesUsuarioCapaNegocioDTO credenciales)
        {
            Usuario? usuario;
            CDUsuarioDatosIniciales modelo;
            UsuarioAutenticadoDTO usuarioDTO;
            Idioma? idioma;

            //Consultar la base de datos por login indicado.
            usuario = this._consultasUsuarios.GetUsuarioPorLogin(credenciales.NombreDeUsuario);

            //Si el usuario no existe en la base de datos, devolver error.
            if (usuario == null)
            {
                return new ResultadoAutentificacion(ResultadoAutentificacion.CodigosErrorAutentificacion.ERR_NoEsUsuarioApp);
            }

            //Si la contraseña hasheada no coinciden, devolver error.
            if (!usuario.Contraseña.Equals(credenciales.Contraseña))
            {
                return new ResultadoAutentificacion(ResultadoAutentificacion.CodigosErrorAutentificacion.ERR_NoEsUsuarioApp);
            }

            //Cargar el usuario actual más sus datos relacionados (permisos de acceso).
            modelo = new CDUsuarioDatosIniciales(this._con);
            modelo.Cargar(usuario.Id);
            usuario = modelo.Usuario;
            idioma = this._con.CargarIdioma(usuario.IdiomaId);
            if (idioma == null)
            {
                idioma = this._con.CargarIdioma(Codigos.Idiomas.Español);
            }

            usuarioDTO = new UsuarioAutenticadoDTO()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellido1 + " " + usuario.Apellido2,
                Login = usuario.Login,
                Permisos = usuario.Permisos,
                CorreoE = usuario.CorreoE,
                IdiomaActualId = usuario.IdiomaId,
                CodigoIdiomaActual = idioma!.Codigo,
                FechaNacimiento = usuario.FechaNacimiento,
                Telefono = usuario.Telefono,
            };

            return new ResultadoAutentificacion(usuarioDTO);
        }

        #endregion
    }
}
