namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public class Configuracion
    {
        public string strCadenaConexion { get; set; }

        public int iSqlQueryTimeOut { get; set; }


        public Configuracion(string strConnectionString)
        {
            strCadenaConexion = strConnectionString;
            iSqlQueryTimeOut = 30;
        }

        public Configuracion(string strConnectionString, int iTimeOutSql)
        {
            strCadenaConexion = strConnectionString;
            iSqlQueryTimeOut = iTimeOutSql;
        }
    }
}
