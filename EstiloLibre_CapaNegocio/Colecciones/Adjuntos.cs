using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Adjuntos : ListaObjetosBD<Adjunto>
    {
        public Adjuntos() : base() { }
        public Adjuntos(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Adjuntos(IEnumerable<Adjunto> lista) : base(lista) { }
    }
}
