using Serilog;

namespace EstiloLibre
{
    public class Program
    {
        public static readonly string EspacioDeNombres = typeof(Program).Namespace!;
        public static readonly string NombreAplicacion = EspacioDeNombres;

        public static int Main(string[] args)
        {
            IConfiguration configuracionGeneral;
            IHost host;

            //Cargar la configuración general de la aplicación (appsettings.json).
            configuracionGeneral = CargarConfiguracionGeneral();

            try
            {
                //Crear el host.
                host = BuildHostBuilder(args, configuracionGeneral);

                //Arrancar la aplicación.
                host.Run();                

                //Salir sin indicar error al sistema operativo.
                return 0;
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
                return 1;
            }
        }

        public static IHost BuildHostBuilder(string[] args, IConfiguration configuracionGeneral)
        {
            return Host.CreateDefaultBuilder(args)

                //Especificar que la configuración de la clase Startup sea la recibida por argumento en lugar
                //de que .NET la cargue de nuevo por su cuenta. Esto último no interesa porque no modificará
                //ciertos valores de claves de configuración que sí se hace en esta clase Program.cs
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuracionGeneral);
                })

                //Indicar la clase Startup.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

                //Borrar todos los registros de los loggers que vienen prerregistrados.
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
            .Build();
        }

        private static IConfiguration CargarConfiguracionGeneral()
        {
            IConfigurationBuilder configBuilder;
            string strPathBase;

            //En el entorno de PRD la ubicación del fichero de configuración puede estar en una variable 
            //de entorno (PRD).
            strPathBase = Environment.GetEnvironmentVariable("apps_config_path") ?? Directory.GetCurrentDirectory();

            //Cargar el fichero appsettings.json.
            configBuilder = new ConfigurationBuilder()
                .SetBasePath(strPathBase)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //Devolver el objeto de configuración.
            return configBuilder.Build();
        }
    }
}
