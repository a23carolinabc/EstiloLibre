using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Colores : ListaObjetosBD<Color>
    {
        public Colores() : base() { }
        public Colores(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Colores(IEnumerable<Color> lista) : base(lista) { }
    }
}
