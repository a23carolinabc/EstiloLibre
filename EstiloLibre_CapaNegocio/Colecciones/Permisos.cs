using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Permisos : ListaObjetosBD<Permiso>
    {
        public Permisos() : base() { }
        public Permisos(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Permisos(IEnumerable<Permiso> lista) : base(lista) { }
    }
}
