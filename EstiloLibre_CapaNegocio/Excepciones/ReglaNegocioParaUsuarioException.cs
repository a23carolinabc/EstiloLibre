namespace EstiloLibre_CapaNegocio.Excepciones
{
    public class ReglaNegocioParaUsuarioException : ReglaNegocioException
    {
        private string Mensaje { get; set; }

        internal ReglaNegocioParaUsuarioException(string strCodigo) : base(strCodigo) { }

        internal ReglaNegocioParaUsuarioException(string strCodigo, params object[] aDatos) : base(strCodigo, aDatos) { }
    }
}
