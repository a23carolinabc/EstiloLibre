using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class PermisosUsuarios : ListaObjetosBD<PermisoUsuario>
    {
        public PermisosUsuarios() : base() { }
        public PermisosUsuarios(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public PermisosUsuarios(IEnumerable<PermisoUsuario> lista) : base(lista) { }
    }
}
