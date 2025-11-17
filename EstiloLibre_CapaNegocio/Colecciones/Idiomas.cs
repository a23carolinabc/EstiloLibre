using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones;

public class Idiomas : ListaObjetosBD<Idioma>
{
    public Idiomas() : base() { }
    public Idiomas(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
    public Idiomas(IEnumerable<Idioma> lista) : base(lista) { }
}
