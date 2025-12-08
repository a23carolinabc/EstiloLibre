using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdPrendasSaveData
    {
        public class Dtos
        {
            public class PrendaSaveDataDTO
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
            }
        }
    }
}
