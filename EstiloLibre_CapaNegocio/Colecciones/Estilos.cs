using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Estilos : ListaObjetosBD<Estilo>
    {
        public Estilos() : base() { }
        public Estilos(int iCapacidadInicial) : base(iCapacidadInicial) { }
        public Estilos(IEnumerable<Estilo> lista) : base(lista) { }
    }
}