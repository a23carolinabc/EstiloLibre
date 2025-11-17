namespace EstiloLibre_CapaNegocio.ObjetosDTO
{
    public class ControlItemDTO
    {
        public Object Id { get; set; }
        public string Nombre { get; set; }

        public ControlItemDTO() { }
        public ControlItemDTO(string nombre, Object id) 
        { 
            Id = id; Nombre = nombre; 
        }
    }
}
