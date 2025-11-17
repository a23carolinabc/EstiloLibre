using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Usuarios : ListaObjetosBD<Usuario>
    {
        public Usuarios() : base() { }
        public Usuarios(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Usuarios(IEnumerable<Usuario> lista) : base(lista) { }
    }
}
