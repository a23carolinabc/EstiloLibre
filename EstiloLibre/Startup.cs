using EstiloLibre.Configuraciones;
using EstiloLibre.Filtros;
using EstiloLibre.Hubs;
using EstiloLibre.Servicios;
using EstiloLibre_CapaNegocio.Configuracion;
using EstiloLibre_CapaNegocio.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace EstiloLibre
{
    public class Startup
    {
        private IConfiguration _configuracion;
        private IWebHostEnvironment _entornoActual;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this._configuracion = configuration;
            this._entornoActual = env;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //Control general de excepciones.
                options.Filters.Add(typeof(FiltroGlobalExcepciones));
            });
            services.AddScoped<FiltroGlobalExcepciones>();

            services
               .AddSwagger()
               .ConfigurarPolicitaCORS(this._configuracion, this._entornoActual)
               .RegistrarServiciosVarios()
               .RegistrarServiciosSignalR()
               .RegistrarServiciosAutenticacion(this._configuracion)
               .RegistrarElementosMediador()
               .RegistrarElementosCapaNegocio(this._configuracion);

            services.AddSpaStaticFiles(configuracion =>
            {
                configuracion.RootPath = "AplicacionCliente";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "Docker")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EstiloLibre.API v1"));
            }
            else
            {
                app.UseHsts();
            }

            if (env.EnvironmentName != "Docker")
            {
                app.UseHttpsRedirection();
            }

            //SignalR
            app.UseResponseCompression();
            
            if (!env.IsDevelopment() && env.EnvironmentName != "Docker")
            {
                var secondaryProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "AplicacionCliente"));
                env.WebRootFileProvider = new CompositeFileProvider(env.WebRootFileProvider, secondaryProvider);
                app.UseBlazorFrameworkFiles();

                app.UseStaticFiles();
            }

            app.UseRouting();

            //Se crea un método a parte para poderlo sobrescribir en un Startup personalizado para las pruebas
            //funcionales.
            //NOTA: Esto debe estar entre UseRouting y UseEndpoints.
            this.UsarAutenticacion(app);

            if (env.IsDevelopment() || env.EnvironmentName == "Docker")
            {
                app.UseCors("ClienteBlazorCORS");
            }

            app.UseEndpoints(endpoints =>
                    {
                        //SignalR
                        endpoints.MapControllers();
                        endpoints.MapHub<ConexionFrontHUB>("hub/conexion");                      
                    });

#if RELEASE
            //Configurar aplicación cliente (Blazor).
            this.UsarBlazor(app, env, this._configuracion);
#endif
        }

        protected virtual void UsarBlazor(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuracion)
        {
            //Redirigir todas las peticiones que no sean al API a la aplicación de Angular.
            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api")
                        && !x.Request.Path.Value.StartsWith("/hub")
                        && x.Request.Method == HttpMethod.Get.Method //Para evitar vulnerabilidad Server Error Message
                        && !x.Request.Path.Value.Contains("..;"), //Para evitar detección de vulnerabilidad Path Normalization Conflict
            builder =>
            {
                builder.UseSpa(spa =>
                {
                    //Localización de los archivos de Blazor.
                    if (env.IsProduction())
                    {
                        //En Producción:
                        spa.Options.SourcePath = "AplicacionCliente";
                    }
                });
            });
        }

        protected virtual void UsarAutenticacion(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }



    internal static class MetodosExtension
    {
        public static IServiceCollection RegistrarElementosCapaNegocio(this IServiceCollection servicios, IConfiguration configuracion)
        {            
            //Registrar el objeto de configuración de la capa de negocio.
            servicios.AddSingleton<Configuracion>(serviceProvider =>
            {
                Configuracion config;
                IConfigurationSection configCapaNegocio;
                DatosConfiguracion datosConfiguracion;

                configCapaNegocio = configuracion.GetSection("CapaNegocio");
                datosConfiguracion = new DatosConfiguracion()
                {
                    RutaGestorArchivos = configCapaNegocio["RutaGestorArchivos"]!,
                    CadenaDeConexion = configCapaNegocio["CadenaDeConexion"]!,
                    TimeOutConsultasSql = int.Parse(configCapaNegocio["TimeOutComandosConsultasEnSeg"]!),
                };
                config = new Configuracion(datosConfiguracion);
                return config;
            });


            //Añadir al contenedor IOC el filtro de apertura y cierre automático del objeto Conexion de la 
            //capa de negocio. De esta forma, todo lo que herede de ControladorBase o añada el filtro de
            //forma explícita como atributo, no tendrá que preocuparse de abrir y cerrar dicho objeto.
            servicios.AddScoped<FiltroAbrirCerrarConexion>();

            //Filtro para establecer los datos de sesión del usuario actual (idioma, etc).
            servicios.AddScoped<FiltroEstablecerDatosSesion>();

            //Añadir servicios de capa de negocio.
            servicios.AddServiciosCapaNegocio();

            //Devolver referencia a los servicios para poder concatenar llamadas.
            return servicios;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection servicios)
        {
            servicios.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Base API",
                    Version = "v1",
                    Description = "API de servicio para la aplicación SPA SXA.AngularUI"
                });
                c.CustomSchemaIds(type => type.ToString());
            });

            return servicios;
        }


        public static IServiceCollection ConfigurarPolicitaCORS(this IServiceCollection servicios, IConfiguration configuration, IWebHostEnvironment entornoActual)
        {
            if (entornoActual.IsDevelopment() || entornoActual.EnvironmentName == "Docker")
            {
                servicios.AddCors(opciones =>
                {
                    opciones.AddPolicy("ClienteBlazorCORS", builder =>
                    {
                        builder.WithOrigins(configuration.GetSection("AplicacionCliente")["Origen1"]!,
                                            configuration.GetSection("AplicacionCliente")["Origen2"]!)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .SetIsOriginAllowed((host) => host == configuration.GetSection("AplicacionCliente")["Origen1"]
                                                       || host == configuration.GetSection("AplicacionCliente")["Origen2"]);
                    });
                });
            }
            return servicios;
        }

        public static IServiceCollection RegistrarServiciosVarios(this IServiceCollection servicios)
        {
            //Proveedor de acceso al contexto HTTP (cabeceras, body, request, etc).
            servicios.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Servicio de memoria caché compartida para todas las peticiones.
            servicios.AddMemoryCache();

            //Devolver referencia a los servicios para poder concatenar llamadas.
            return servicios;
        }

        public static IServiceCollection RegistrarServiciosSignalR(this IServiceCollection servicios)
        {
            servicios.AddSignalR();

            servicios.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]);
            });

            return servicios;
        }

        public static IServiceCollection RegistrarElementosMediador(this IServiceCollection servicios)
        {
            servicios.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Devolver referencia a los servicios para poder concatenar llamadas.
            return servicios;
        }

        public static IServiceCollection RegistrarServiciosAutenticacion(this IServiceCollection servicios, IConfiguration configuracion)
        {
            ConfiguracionAutenticacion configAutenticacion;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //Registrar configuración.
            servicios.Configure<ConfiguracionAutenticacion>(configAutenticacion =>
            {
                configuracion.Bind("Autenticacion", configAutenticacion);
            });

            //Recuperar configuración.
            configAutenticacion = configuracion.GetSection("Autenticacion").Get<ConfiguracionAutenticacion>()!;

            //Registrar el middleware de autenticación.
            servicios.AddAuthentication(opciones =>
            {
                opciones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opciones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opciones =>
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configAutenticacion.EmisorToken,
                    ValidAudience = configAutenticacion.AudienciaToken,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configAutenticacion.TokenJWT_ClaveSecreta))
                };
            });

            //Servicio verificador de usuarios.
            servicios.AddScoped<ServicioIdentidadTokenJwt>();

            //Filtro para establecer en el objeto Conexion de la capa de negocio la
            //información del usuario actual.
            servicios.AddScoped<FiltroIdentificacion>();

            //Servicios obligatorios.
            //servicios.AddScoped<FiltroParaDesconexion>();
            servicios.AddScoped<ServicioCredencialesCabecera>();
            servicios.AddScoped<ServicioTokenPropio>();
            //servicios.AddScoped<DatosSesion>();

            //Devolver la clase de servicios para que se puedan concatenar llamadas.
            return servicios;
        }       
        
    }
}
