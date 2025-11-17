//using EstiloLibre_CapaNegocio.AccesoBD;

//namespace EstiloLibre_CapaNegocio.Base
//{
//    public class ObjetoTraduccionDAO : DAO
//    {
//        internal readonly string strNombreColumnaEnlaceTraducciones;

//        #region ***** CONSTRUCTORES *****
//        public ObjetoTraduccionDAO(Conexion conexion, string strTablaTraduccion, string strNombreColumnaEnlaceTraducciones) 
//            : base(conexion, strTablaTraduccion)
//        {
//            this.strNombreColumnaEnlaceTraducciones = strNombreColumnaEnlaceTraducciones;
//        }
//        #endregion

//        #region ***** MÉTODOS PÚBLICOS *****
//        public ListaObjetosBD<ObjetoTraduccionBD> CrearListaObjetosBD()
//        {
//            return new ObjetosTraduccionBD();
//        }

//        public ListaObjetosBD<ObjetoTraduccionBD> CrearListaObjetosBD(int iCapacidadInicial)
//        {
//            return new ObjetosTraduccionBD(iCapacidadInicial);
//        }

//        public override ObjetoTraduccionBD CrearObjetoBD()
//        {
//            return new ObjetoTraduccionBD(this);
//        }
//        #endregion
//    }
//}
