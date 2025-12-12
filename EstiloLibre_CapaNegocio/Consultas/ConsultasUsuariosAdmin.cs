using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuarios.DTOs;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasUsuariosAdmin.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuariosAdmin
    {
        #region ***** PROPIEDADES INTERNAS *****

        private readonly Conexion _con;
        private readonly UsuariosDAO _dao;
        private readonly ServicioCombos _servicioCombos;
        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTOR *****

        public ConsultasUsuariosAdmin(Conexion con,
                                      ServicioCombos servicioCombos,
                                      ServicioAlmacenamiento servicioAlmacenamiento)
        {
            this._con = con;
            this._dao = new UsuariosDAO(con);
            this._servicioCombos = servicioCombos;
            this._servicioAlmacenamiento = servicioAlmacenamiento;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        /// <summary>
        /// Obtiene los datos necesarios para crear un nuevo usuario administrador
        /// </summary>
        public UsuarioAdminAddNewDTO GetDatosAddNew()
        {
            CDUsuariosAdminAddNew cd;
            UsuarioAdminAddNewDTO dto;

            cd = new CDUsuariosAdminAddNew(this._con);
            cd.Cargar();

            dto = this.GetDatosParaAddNew(cd);
            return dto;
        }

        /// <summary>
        /// Obtiene los datos completos de un usuario administrador para edición
        /// </summary>
        public async Task<UsuarioAdminShowDataDTO> GetDatosShowData(int iUsuarioId)
        {
            CDUsuariosAdminShowData cd;
            UsuarioAdminShowDataDTO dto;

            cd = new CDUsuariosAdminShowData(this._con);
            cd.Cargar(iUsuarioId);

            dto = await this.GetDatosParaShowData(cd);
            return dto;
        }

        /// <summary>
        /// Obtiene un listado de usuarios administradores con búsqueda opcional
        /// </summary>
        public async Task<IEnumerable<UsuarioAdminResumenDTO>> GetListadoUsuariosAdmin(string? strTextoBusqueda, string? strTipoBusqueda)
        {
            CDUsuariosAdminListado cd;
            List<UsuarioAdminResumenDTO> lista;
            Adjuntos adjuntos;
            Permisos permisosUsuario;

            cd = new CDUsuariosAdminListado(this._con);
            cd.Cargar(strTextoBusqueda, strTipoBusqueda);

            lista = new List<UsuarioAdminResumenDTO>();

            foreach (Usuario usuario in cd.Usuarios)
            {
                UsuarioAdminResumenDTO dto;

                dto = new UsuarioAdminResumenDTO();
                dto.Id = usuario.Id;
                dto.Login = usuario.Login;
                dto.NombreCompleto = $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}".Trim();
                dto.CorreoE = usuario.CorreoE;

                // Cargar permisos del usuario
                permisosUsuario = this._con.CargarPermisosUsuario(usuario.Id);
                dto.PermisosAsignados = permisosUsuario.Select(p => p.Codigo).ToList();

                // Cargar imagen del usuario
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, usuario.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    dto.FotoBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                lista.Add(dto);
            }

            return lista;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private UsuarioAdminAddNewDTO GetDatosParaAddNew(CDUsuariosAdminAddNew cd)
        {
            UsuarioAdminAddNewDTO objeto;
            List<PermisoDTO> permisosDTO;

            objeto = new UsuarioAdminAddNewDTO();
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);

            // Convertir permisos a DTOs
            permisosDTO = new List<PermisoDTO>();
            foreach (Permiso permiso in cd.Permisos)
            {
                permisosDTO.Add(new PermisoDTO(permiso, false));
            }
            objeto.PermisosDisponibles = permisosDTO;

            return objeto;
        }

        private async Task<UsuarioAdminShowDataDTO> GetDatosParaShowData(CDUsuariosAdminShowData cd)
        {
            UsuarioAdminShowDataDTO objeto;
            List<PermisoDTO> permisosDTO;
            List<int> permisosAsignadosIds;
            Adjuntos adjuntos;

            objeto = new UsuarioAdminShowDataDTO();
            objeto.Usuario = new UsuarioAdminDTO(cd.Usuario);
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);

            // Obtener IDs de permisos asignados
            permisosAsignadosIds = cd.PermisosAsignados.Select(p => p.Id).ToList();
            objeto.Usuario.PermisosIds = permisosAsignadosIds;

            // Convertir permisos disponibles a DTOs con flag de asignado
            permisosDTO = new List<PermisoDTO>();
            foreach (Permiso permiso in cd.PermisosDisponibles)
            {
                bool bEstaAsignado;

                bEstaAsignado = permisosAsignadosIds.Contains(permiso.Id);
                permisosDTO.Add(new PermisoDTO(permiso, bEstaAsignado));
            }
            objeto.PermisosDisponibles = permisosDTO;

            // Cargar imagen del usuario
            if (objeto.Usuario.Id > 0)
            {
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, objeto.Usuario.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    objeto.Usuario.FotoBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }
            }

            return objeto;
        }

        #endregion
    }
}
