using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    /// <summary>
    /// Comando para eliminar un usuario administrador
    /// </summary>
    public class CmdUsuariosAdminDelete : ComandoBase, IRequest
    {
        public int UsuarioId { get; set; }

        public CmdUsuariosAdminDelete(int iUsuarioId) : base()
        {
            this.UsuarioId = iUsuarioId;
        }
    }

    public class PcmdUsuariosAdminDelete : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosAdminDelete>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdUsuariosAdminDelete(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public Task Handle(CmdUsuariosAdminDelete comando, CancellationToken cancellationToken)
        {
            Usuario? usuario;
            Adjuntos adjuntos;

            try
            {
                this.con.BeginTrans();

                // Cargar el usuario
                usuario = this.con.CargarUsuario(comando.UsuarioId);
                if (usuario is null)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                }

                // Eliminar adjuntos físicos del usuario
                adjuntos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, usuario.Id);
                foreach (Adjunto adjunto in adjuntos)
                {
                    // Eliminar archivo físico
                    this._servicioAlmacenamiento.EliminarArchivo(adjunto);

                    // Eliminar registro de BD
                    adjunto.Eliminar();
                }

                // Eliminar permisos del usuario
                this.con.EliminarPermisosDeUsuario(usuario.Id);

                // Eliminar usuario
                usuario.Eliminar();

                // Confirmar la transacción
                this.con.CommitTrans();

                return Task.FromResult(Unit.Value);
            }
            catch
            {
                // Fallo detectado. Deshacer transacción y relanzar la excepción
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}
