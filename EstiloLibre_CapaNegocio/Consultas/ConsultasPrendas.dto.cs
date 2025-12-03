using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasPrendas
    {
        public class Dtos
        {
            public class PrendasAddNewDto
            {
                public IEnumerable<ControlItem> Estaciones { get; set; }
                public IEnumerable<ControlItem> Marcas { get; set; }
                public IEnumerable<ControlItem> Materiales { get; set; }
                public IEnumerable<ControlItem> Tallas { get; set; }
                public IEnumerable<ControlItem> Categorias { get; set; }
                public IEnumerable<ControlItem> Colores { get; set; }
                public IEnumerable<ControlItem> Estados { get; set; }
            }
        }
    }
}
