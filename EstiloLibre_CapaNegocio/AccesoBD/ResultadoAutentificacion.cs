using EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public class ResultadoAutentificacion
    {
        #region ***** PROPIEDADES *****
        public enum CodigosErrorAutentificacion
        {
            OK,
            ERR_NoAutenticadoExternamente,
            ERR_ContraseñaInvalida,
            ERR_CuentaBloqueada,
            ERR_ContraseñaNoSegura,
            ERR_ContraseñaRepetida,
            ERR_NoEsUsuarioApp,
            ERR_CuentaBaja,
        }
        public bool Correcto { get; set; }
        public CodigosErrorAutentificacion CodigoError { get; }
        public UsuarioAutenticadoDTO UsuarioAutenticado { get; }
        #endregion

        #region ***** CONSTRUCTORES *****

        public ResultadoAutentificacion(CodigosErrorAutentificacion strCodigoError)
        {
            if (strCodigoError == CodigosErrorAutentificacion.OK)
                Correcto = true;
            else
                Correcto = false;
            CodigoError = strCodigoError;
        }

        public ResultadoAutentificacion(UsuarioAutenticadoDTO usuario) : this(CodigosErrorAutentificacion.OK)
        {
            UsuarioAutenticado = usuario;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public string GetCodigoError()
        {
            return Enum.GetName(typeof(CodigosErrorAutentificacion), CodigoError);
        }

        #endregion
    }
}
