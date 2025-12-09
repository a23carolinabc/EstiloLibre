using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasPrendas
    {
        public class DTOs
        {
            public class PrendasAddNewDTO
            {
                public IEnumerable<ControlItem> Estaciones { get; set; }
                public IEnumerable<ControlItem> Marcas { get; set; }
                public IEnumerable<ControlItem> Materiales { get; set; }
                public IEnumerable<ControlItem> Tallas { get; set; }
                public IEnumerable<ControlItem> Categorias { get; set; }
                public IEnumerable<ControlItem> Colores { get; set; }
                public IEnumerable<ControlItem> Estados { get; set; }
            }

            public class PrendasShowDataDTO
            {
                public PrendaDTO Prenda { get; set; }
                public IEnumerable<ControlItem> Estaciones { get; set; }
                public IEnumerable<ControlItem> Marcas { get; set; }
                public IEnumerable<ControlItem> Materiales { get; set; }
                public IEnumerable<ControlItem> Tallas { get; set; }
                public IEnumerable<ControlItem> Categorias { get; set; }
                public IEnumerable<ControlItem> Colores { get; set; }
                public IEnumerable<ControlItem> Estados { get; set; }
            }

            public class PrendaDTO
            {
                public int Id { get; set; }
                public int ColorId { get; set; }
                public int CategoriaId { get; set; }
                public int EstadoId { get; set; }
                public int TallaId { get; set; }
                public int MaterialId { get; set; }
                public int? MarcaId { get; set; }
                public int? EstacionId { get; set; }
                public decimal? Precio { get; set; }
                public string? EnlaceCompra { get; set; }
                public DateTime? FechaCompra { get; set; }
                public string FotoBase64 { get; set; }

                public PrendaDTO() { }
                public PrendaDTO(Prenda prenda) 
                {
                    this.Id = prenda.Id;
                    this.ColorId = prenda.ColorId;
                    this.CategoriaId = prenda.CategoriaId;
                    this.EstadoId = prenda.EstadoId;
                    this.TallaId = prenda.TallaId;
                    this.MaterialId = prenda.MaterialId;
                    this.MarcaId = prenda.MarcaId;
                    this.EstacionId = prenda.EstacionId;
                    this.Precio = prenda.Precio;
                    this.EnlaceCompra = prenda.EnlaceCompra;
                    this.FechaCompra = prenda.FechaCompra;
                }
            }
        }
    }
}
