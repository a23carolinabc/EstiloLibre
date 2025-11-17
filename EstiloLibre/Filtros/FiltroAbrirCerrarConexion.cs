using Microsoft.AspNetCore.Mvc.Filters;
using EstiloLibre_CapaNegocio.AccesoBD;

namespace EstiloLibre.Filtros
{
    public class FiltroAbrirCerrarConexion : IResourceFilter
    {
        private readonly Conexion _con;

        public FiltroAbrirCerrarConexion(Conexion con)
        {
            _con = con;
        }
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _con.Conectar();
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _con.Desconectar();
        }
    }
}
