using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;
using static EstiloLibre_CapaNegocio.Comandos.CmdUsuariosAdminSaveData.DTOs;

namespace EstiloLibre_CapaNegocio.Comandos
{
    /// <summary>
    /// Comando para guardar o actualizar un usuario administrador
    /// </summary>
    public partial class CmdUsuariosAdminSaveData : ComandoBase, IRequest<int>
    {
        public UsuarioAdminSaveDataDTO Usuario { get; set; }

        public CmdUsuariosAdminSaveData(UsuarioAdminSaveDataDTO usuarioSaveData) : base()
        {
            this.Usuario = usuarioSaveData;
        }
    }

    public class PcmdUsuariosAdminSaveData : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosAdminSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdUsuariosAdminSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdUsuariosAdminSaveData comando, CancellationToken cancellationToken)
        {
            Usuario? usuario;
            Usuario? usuarioExistente;
            Adjunto adjunto;
            byte[] byImagen;
            bool bEsActualizacion;
            Permisos permisosSeleccionados;
            bool bTienePermisoAdmin;

            bEsActualizacion = false;

            try
            {
                // Envolver todo el proceso en una transacción
                this.con.BeginTrans();

                // Validar que al menos tenga el permiso ADMIN
                permisosSeleccionados = this.con.CargarPermisos(comando.Usuario.PermisosIds);
                bTienePermisoAdmin = permisosSeleccionados.Any(p => p.Codigo == Codigos.Permisos.ADMIN);

                if (!bTienePermisoAdmin)
                {
                    throw new ReglaNegocioParaUsuarioException("ERR_UsuarioAdminDebeeTenerPermisoADMIN");
                }

                // Buscar si el objeto ya estaba registrado en BD
                if (comando.Usuario.Id > 0)
                {
                    usuario = this.con.CargarUsuario(comando.Usuario.Id);
                    if (usuario == null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                    }

                    bEsActualizacion = true;
                }
                else
                {
                    // Validar que el login no esté en uso
                    usuarioExistente = this.con.CargarUsuario(comando.Usuario.Login);
                    if (usuarioExistente != null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_LoginEnUso");
                    }

                    // Validar que el correo no esté en uso
                    usuarioExistente = this.con.CargarUsuarioPorCorreo(comando.Usuario.CorreoE);
                    if (usuarioExistente != null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_CorreoEnUso");
                    }

                    usuario = this.con.CrearUsuario();

                    // Asignar la contraseña hasheada (obligatoria en creación)
                    if (string.IsNullOrEmpty(comando.Usuario.Contraseña))
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ContraseñaRequerida");
                    }

                    usuario.Contraseña = UtilsVarios.GenerarHash(comando.Usuario.Contraseña);
                }

                // Transferir propiedades del DTO al objeto de BD
                usuario.Login = comando.Usuario.Login;
                usuario.Nombre = comando.Usuario.Nombre;
                usuario.Apellido1 = comando.Usuario.Apellido1;
                usuario.Apellido2 = comando.Usuario.Apellido2;
                usuario.CorreoE = comando.Usuario.CorreoE;
                usuario.IdiomaId = comando.Usuario.IdiomaId;
                usuario.Publico = false; // Los admin no son públicos
                usuario.Telefono = comando.Usuario.Telefono;
                usuario.FechaNacimiento = comando.Usuario.FechaNacimiento;

                // Si es actualización y hay nueva contraseña, actualizar el hash
                if (bEsActualizacion && !string.IsNullOrEmpty(comando.Usuario.Contraseña))
                {
                    usuario.Contraseña = UtilsVarios.GenerarHash(comando.Usuario.Contraseña);
                }

                // Guardar usuario
                usuario.Guardar();

                // Gestionar permisos del usuario
                this.con.EliminarPermisosDeUsuario(usuario.Id);
                this.con.AsignarPermisosAUsuario(usuario.Id, comando.Usuario.PermisosIds);

                // Gestionar foto del usuario
                if (!string.IsNullOrEmpty(comando.Usuario.FotoBase64))
                {
                    if (bEsActualizacion)
                    {
                        Adjuntos adjuntosAntiguos;

                        adjuntosAntiguos = this.con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, usuario.Id);

                        foreach (Adjunto adjuntoAntiguo in adjuntosAntiguos)
                        {
                            // Eliminar archivo físico
                            this._servicioAlmacenamiento.EliminarArchivo(adjuntoAntiguo);

                            // Eliminar registro de BD
                            adjuntoAntiguo.Eliminar();
                        }
                    }

                    // Procesar imagen (redimensionar, convertir a WebP)
                    byImagen = await this._servicioAlmacenamiento.ProcesarImagen(comando.Usuario.FotoBase64);

                    // Crear nuevo adjunto en BD
                    adjunto = this.con.CrearAdjunto();
                    adjunto.ClaseObjetoId = Codigos.ClasesObjetos.Usuario;
                    adjunto.ObjetoId = usuario.Id;
                    adjunto.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjunto.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto
                    adjunto.Guardar();

                    // Guardar archivo físico comprimido
                    await this._servicioAlmacenamiento.GuardarArchivo(adjunto, byImagen);
                }

                // Confirmar transacción
                this.con.CommitTrans();

                // Devolver id del usuario
                return usuario.Id;
            }
            catch
            {
                this.con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}