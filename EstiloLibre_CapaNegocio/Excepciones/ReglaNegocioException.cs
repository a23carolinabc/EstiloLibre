namespace EstiloLibre_CapaNegocio.Excepciones
{
    public class ReglaNegocioException : ApplicationException
    {
        public string Codigo { get; set; }
        private string Mensaje { get; set; }
        public object[] Datos { get; set; }

        internal ReglaNegocioException(string strCodigo) : base(strCodigo)
        {
            this.Codigo = strCodigo;
        }

        internal ReglaNegocioException(string strCodigo, params object[] aDatos) : base(strCodigo)
        {
            this.Codigo = strCodigo;
            this.Datos = aDatos;
        }

    }
}
