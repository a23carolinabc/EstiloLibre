namespace EstiloLibre_CapaNegocio.Excepciones
{
    public class CapaNegocioException : ApplicationException
    {
        public CapaNegocioException(string msg, Exception excp) : base(msg, excp) { }
        public CapaNegocioException(string msg) : base(msg) { }
    }
}
