using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Consultas;
using EstiloLibre_CapaNegocio.Servicios;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EstiloLibre_CapaNegocio.Utils
{
    public static class ExtensionesGenerales
    {
        public static void AddServiciosCapaNegocio(this IServiceCollection services)
        {
            services.AddScoped<Conexion>();
            AddConsultas(services);
            AddServicios(services);

            services.AddScoped<ServicioAutentificacionPersonas>();            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        #region ***** MÉTODOS PRIVADOS *****        

        private static void AddServicios(this IServiceCollection services)
        {
            //Registrar servicios.
            services.AddScoped<ServicioCombos>();
            services.AddScoped<ServicioAlmacenamiento>();
        }

        private static void AddConsultas(this IServiceCollection services)
        {
            //Registrar consultas.
            services.AddScoped<ConsultasUsuarios>();
            services.AddScoped<ConsultasPrendas>();
            services.AddScoped<ConsultasConjuntos>();
        }
        #endregion
    }
}
