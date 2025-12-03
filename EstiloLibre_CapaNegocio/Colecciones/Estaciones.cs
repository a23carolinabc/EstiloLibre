using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Estaciones : ListaObjetosBD<Estacion>
    {
        public Estaciones() : base() { }
        public Estaciones(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Estaciones(IEnumerable<Estacion> lista) : base(lista) { }
    }
}
