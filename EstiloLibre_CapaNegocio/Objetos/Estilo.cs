using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Estilos)]
    public class Estilo : ObjetoBD
    {
        #region ****** PROPIEDADES *****

        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Estilo() : base() { }

        public Estilo(EstilosDAO dao) : base(dao) { }

        #endregion
    }
}