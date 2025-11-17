namespace EstiloLibre_CapaNegocio.Base
{
    public class PropiedadObjetoBD
    {
        private string Nombre { get; set; }

        private bool EsClavePrimaria { get; set; }

        private bool EsAutonumerico { get; set; }

        private bool EsActualizable { get; set; }

        private bool CargarTrasActualizar {  get; set; }

        public PropiedadObjetoBD(string strNombre)
        {
            Nombre = strNombre;
        }

        public PropiedadObjetoBD(string strNombre, bool bEsClavePrimaria, bool bEsAutonumerico, bool bActualizable, bool bCargarTrasActualizar)
        {
            Nombre = strNombre;
            EsClavePrimaria = bEsClavePrimaria;
            EsAutonumerico = bEsAutonumerico;
            EsActualizable = bActualizable;
            CargarTrasActualizar = bCargarTrasActualizar;
        }

        public PropiedadObjetoBD(string strNombre, bool bActualizable, bool bCargarTrasActualizar)
        {
            Nombre = strNombre;
            EsClavePrimaria = false;
            EsAutonumerico = false;
            EsActualizable = bActualizable;
            CargarTrasActualizar = bCargarTrasActualizar;
        }
    }
}
