using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasConjuntos
    {
        public class DTOs
        {
            public class ConjuntosAddNewDTO
            {
                public IEnumerable<ControlItem> Estaciones { get; set; }
                public IEnumerable<ControlItem> Estilos { get; set; }
                public IEnumerable<ControlItem> Colores { get; set; }
            }

            public class ConjuntosShowDataDTO
            {
                public ConjuntoDTO? Conjunto { get; set; }
                public IEnumerable<ControlItem> Estaciones { get; set; }
                public IEnumerable<ControlItem> Estilos { get; set; }
                public IEnumerable<ControlItem> Colores { get; set; }
            }

            public class ConjuntoDTO
            {
                public int Id { get; set; }
                public int? EstacionId { get; set; }
                public int? EstiloId { get; set; }
                public string? Descripcion { get; set; }
                public string? Etiquetas { get; set; }
                public bool EsFavorito { get; set; }
                public string DatosComposicion { get; set; }
                public string? NotasPersonales { get; set; }
                public string? ImagenBase64 { get; set; }
                public List<int>? PrendasIds { get; set; }

                public ConjuntoDTO() { }                

                public ConjuntoDTO(Conjunto conjunto)
                {
                    this.Id = conjunto.Id;
                    this.EstacionId = conjunto.EstacionId;
                    this.EstiloId = conjunto.EstiloId;
                    this.Descripcion = conjunto.Descripcion;
                    this.Etiquetas = conjunto.Etiquetas;
                    this.EsFavorito = conjunto.EsFavorito;
                    this.DatosComposicion = conjunto.DatosComposicion;
                    this.NotasPersonales = conjunto.NotasPersonales;
                    this.PrendasIds = new List<int>();
                }
            }

            public class PrendaConImagenDTO
            {
                public int Id { get; set; }
                public string? ImagenBase64 { get; set; }
                public string? NombreCategoria { get; set; }
                public string? NombreColor { get; set; }
            }
        }
    }
}
