using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using MediatR;

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdUsuariosSaveData
        : ComandoBase, IRequest<int>
    {
        public DTOs.UsuarioSaveDataDTO Usuario { get; set; }
        public CmdUsuariosSaveData(DTOs.UsuarioSaveDataDTO usuarioSaveData) : base([Codigos.Permisos.USER])
        {
            this.Usuario = usuarioSaveData;
        }
    }

    public class PcmdUsuariosSaveData
        : ProcesadorDeComandoBase, IRequestHandler<CmdUsuariosSaveData, int>
    {
        #region ***** PROPIEDADES *****

        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public PcmdUsuariosSaveData(Conexion con) : base(con)
        {
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<int> Handle(CmdUsuariosSaveData comando, CancellationToken cancellationToken)
        {
            Usuario? usuario;
            Adjunto adjunto;
            byte[] byImagen;
            bool bEsActualizacion;

            bEsActualizacion = false;

            base.VericarPermisos(comando);

            try
            {
                //Envolver todo el proceso en una transacción.
                con.BeginTrans();

                //Buscar si el objeto ya estaba registrado en BD.
                if (comando.Usuario.Id > 0)
                {
                    usuario = con.CargarUsuario(comando.Usuario.Id);
                    if (usuario == null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_ObjetoNoEncontrado");
                    }

                    //Marcamos que es actualización.
                    bEsActualizacion = true;

                    //Si proporciona una nueva contraseña se actualiza
                    if (!string.IsNullOrEmpty(comando.Usuario.ContraseñaActual)
                    && !string.IsNullOrEmpty(comando.Usuario.Contraseña))
                    {
                        string contraseñaActualHasheada;

                        contraseñaActualHasheada = UtilsVarios.GenerarHash(comando.Usuario.ContraseñaActual);

                        //Verificar que la contraseña actual es correcta
                        if (!usuario.Contraseña.Equals(contraseñaActualHasheada))
                        {
                            throw new ReglaNegocioParaUsuarioException("ERR_ContraseñaActualIncorrecta");
                        }

                        //Actualizar la contraseña con la nueva contraseña hasheada
                        usuario.Contraseña = UtilsVarios.GenerarHash(comando.Usuario.Contraseña);
                    }
                }
                else
                {
                    //Comprobar uso del login.
                    usuario = con.CargarUsuario(comando.Usuario.Login);
                    if (usuario != null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_LoginEnUso");
                    }

                    //Comprobar uso del correo electrónico.
                    usuario = con.CargarUsuarioPorCorreo(comando.Usuario.CorreoE);
                    if (usuario != null)
                    {
                        throw new ReglaNegocioParaUsuarioException("ERR_CorreoEnUso");
                    }

                    //Crear usuario.
                    usuario = con.CrearUsuario();

                    //Asignar la contraseña hasheada
                    if (!string.IsNullOrEmpty(comando.Usuario.Contraseña))
                    {
                        usuario.Contraseña = UtilsVarios.GenerarHash(comando.Usuario.Contraseña);
                    }
                }

                //Transferir propiedades del DTO al objeto de BD.
                usuario.Id = comando.Usuario.Id;
                usuario.Login = comando.Usuario.Login;
                usuario.Nombre = comando.Usuario.Nombre;
                usuario.Apellido1 = comando.Usuario.Apellido1;
                usuario.Apellido2 = comando.Usuario.Apellido2;
                usuario.CorreoE = comando.Usuario.CorreoE;
                usuario.IdiomaId = comando.Usuario.IdiomaId;
                usuario.Publico = comando.Usuario.Publico;                      
                usuario.Telefono = comando.Usuario.Telefono;                      
                usuario.FechaNacimiento = comando.Usuario.FechaNacimiento;                      
                
                //Guardar todos los cambios recibidos.
                usuario.Guardar();

                if (!string.IsNullOrEmpty(comando.Usuario.FotoBase64))
                {
                    if (bEsActualizacion)
                    {
                        Adjuntos adjuntosAntiguos = con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, usuario.Id);

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
                    adjunto = con.CrearAdjunto();
                    adjunto.ClaseObjetoId = Codigos.ClasesObjetos.Prenda;
                    adjunto.ObjetoId = usuario.Id;
                    adjunto.TipoAdjuntoId = Codigos.TiposAdjuntos.Imagen;
                    adjunto.Guid = UtilsVarios.GenerarGuid();

                    // Guardar adjunto.
                    adjunto.Guardar();

                    // Guardar archivo físico comprimido
                    await this._servicioAlmacenamiento.GuardarArchivo(adjunto, byImagen);
                }

                //Confirmar transacción.
                con.CommitTrans();

                //Devolver id.
                return usuario.Id;
            }
            catch
            {
                con.RollBackTrans();
                throw;
            }
        }

        #endregion
    }
}
