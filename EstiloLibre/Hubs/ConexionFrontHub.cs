using Microsoft.AspNetCore.SignalR;

namespace EstiloLibre.Hubs
{
    public class ConexionFrontHUB : Hub<IConexionFrontHUB>
    {
        public async override Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ConexionEstablecida(
                $"Conexion establecida {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }
    }

    public interface IConexionFrontHUB
    {
        Task ConexionEstablecida(string mensajeRespuesta);
    }
}