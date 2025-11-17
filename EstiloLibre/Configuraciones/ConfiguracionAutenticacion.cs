namespace EstiloLibre.Configuraciones
{
    public class ConfiguracionAutenticacion
    {
        public int TokenJWT_DuracionTokenEnMinutos { get; set; }
        public string TokenJWT_ClaveSecreta { get; set; }

        public string EmisorToken { get; set; }
        public string AudienciaToken { get; set; }
        public int DuracionTokenEnSegundosParaServicios { get; set; }
    }
}
