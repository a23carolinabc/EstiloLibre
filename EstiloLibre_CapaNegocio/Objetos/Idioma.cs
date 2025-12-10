using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Idiomas)]
    public class Idioma : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Idioma() : base() { }

        public Idioma(IdiomasDAO dao) : base(dao) { }

        #endregion
    }
}