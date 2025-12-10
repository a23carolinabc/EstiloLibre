using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdConjuntosSaveData
    {
        public class DTOs
        {
            public class ConjuntoSaveDataDTO
            {
                public int Id { get; set; }
                public int EstacionId { get; set; }
                public int? EstiloId { get; set; }
                public string? Descripcion { get; set; }
                public string? Etiquetas { get; set; }
                public bool EsFavorito { get; set; }
                public string DatosComposicion { get; set; }
                public string? NotasPersonales { get; set; }
                public List<int> PrendasIds { get; set; }
                public string? ImagenBase64 { get; set; }
            }
        }
    }
}
