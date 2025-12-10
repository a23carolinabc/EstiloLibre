using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Conjunto : ObjetoBD
    {
        #region ****** PROPIEDADES *****

        public int UsuarioId { get; set; }
        public int? EstacionId { get; set; }
        public int? EstiloId { get; set; }
        public string? Descripcion { get; set; }
        public string? Etiquetas { get; set; }
        public bool EsFavorito { get; set; }
        public string DatosComposicion { get; set; }
        public string? NotasPersonales { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Conjunto() : base() { }

        public Conjunto(ConjuntosDAO dao) : base(dao) { }

        #endregion
    }
}