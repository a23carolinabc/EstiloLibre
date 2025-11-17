using Microsoft.AspNetCore.Mvc.Filters;
using EstiloLibre_CapaNegocio.AccesoBD;

namespace EstiloLibre.Filtros
{
    public class FiltroAbrirCerrarConexion : IResourceFilter
    {
        private readonly Conexion _con;

        public FiltroAbrirCerrarConexion(Conexion con)
        {
            this._con = con;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            this._con.Conectar();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            this._con.Desconectar();
        }
    }
}