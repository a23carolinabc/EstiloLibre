namespace EstiloLibre_CapaNegocio.Configuracion
{
    public class Configuracion
    {
        #region ***** PROPIEDADES *****

        public string CadenaDeConexion {  get; set; }
        public int TimeOutConsultasSql { get; set; }
        public string RutaConsultasSQL { get; set; }
        public string RutaPlantillaDocumentos { get; set; }
        public string RutaGestorArchivos { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Configuracion()
        {
            this.EstablecerValoresPorDefecto();
        }
        public Configuracion(DatosConfiguracion datosConfiguracion)
        {
            this.CadenaDeConexion = datosConfiguracion.CadenaDeConexion;
            this.TimeOutConsultasSql = datosConfiguracion.TimeOutConsultasSql;
            this.RutaGestorArchivos = datosConfiguracion.RutaGestorArchivos;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
        public void EstablecerValoresPorDefecto()
        {
            this.TimeOutConsultasSql = 30;
        }
        #endregion
    }
}
