using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.Data;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Prendas)]
    public class Prenda : ObjetoBD
    {
        #region ****** PROPIEDADES *****
        
        public int UsuarioId { get; set; }
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
        
        #endregion

        #region ***** CONSTRUCTORES *****

        public Prenda() : base() { }

        public Prenda(DAO<Prenda> dao) : base(dao) { }        
        
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
              
        #endregion
    }
}
