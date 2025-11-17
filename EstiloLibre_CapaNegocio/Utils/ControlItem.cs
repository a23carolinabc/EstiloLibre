namespace EstiloLibre_CapaNegocio.Utils
{
    public class ControlItem
    {
        public object Id { get; set; }
        public string Nombre { get; set; }

        public ControlItem() { }
        public ControlItem(string nombre, object id)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
