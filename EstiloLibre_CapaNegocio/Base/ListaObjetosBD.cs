namespace EstiloLibre_CapaNegocio.Base
{
    public class ListaObjetosBD<T>: List<T> where T : ObjetoBD  
    {
        #region ***** PROPIEDADES INTERNAS *****

        private int? _iCapacidadInicial;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ListaObjetosBD()             
        {
        }

        public ListaObjetosBD(IEnumerable<T> lstLista) : base(lstLista)
        {
        }

        public ListaObjetosBD(int iCapacidadInicial) : base(iCapacidadInicial)
        {
            this._iCapacidadInicial = iCapacidadInicial;
        }             

        #endregion
    }
}
