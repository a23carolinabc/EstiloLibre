using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using EstiloLibre_CapaNegocio.Utils;
using System.Data;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasAdministracion.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasAdministracion
    {
        #region ***** PROPIEDADES INTERNAS *****

        private readonly Conexion _con;
        private readonly UsuariosDAO _dao;
        private readonly ServicioCombos _servicioCombos;
        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTOR *****

        public ConsultasAdministracion(Conexion con,
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
        /// Obtiene un listado de usuarios normales (sin permiso ADMIN) con búsqueda opcional
        /// </summary>
        public async Task<IEnumerable<UsuarioNormalResumenDTO>> GetListadoUsuariosNormales(string? strTextoBusqueda, string? strTipoBusqueda)
        {
            CDUsuariosListado cd;
            List<UsuarioNormalResumenDTO> lista;
            Adjuntos adjuntos;
            Prendas prendas;
            Conjuntos conjuntos;

            cd = new CDUsuariosListado(this._con);
            cd.Cargar(strTextoBusqueda, strTipoBusqueda);

            lista = new List<UsuarioNormalResumenDTO>();

            foreach (Usuario usuario in cd.Usuarios)
            {
                UsuarioNormalResumenDTO dto;

                dto = new UsuarioNormalResumenDTO();
                dto.Id = usuario.Id;
                dto.Login = usuario.Login;
                dto.NombreCompleto = $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}".Trim();
                dto.CorreoE = usuario.CorreoE;
                dto.FechaNacimiento = usuario.FechaNacimiento;
                dto.Publico = usuario.Publico;

                // Contar prendas del usuario
                prendas = this._con.CargarPrendas(usuario.Id);
                dto.CantidadPrendas = prendas.Count();

                // Contar conjuntos del usuario
                conjuntos = this._con.CargarConjuntos(usuario.Id);
                dto.CantidadConjuntos = conjuntos.Count();

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

        /// <summary>
        /// Obtiene los datos completos de un usuario normal para vista de administración
        /// </summary>
        public async Task<UsuarioNormalShowDataDTO> GetDatosUsuarioNormalParaAdmin(int iUsuarioId)
        {
            CDUsuarioCompleto cd;
            UsuarioNormalShowDataDTO dto;

            cd = new CDUsuarioCompleto(this._con);
            cd.Cargar(iUsuarioId);

            dto = await this.GetDatosParaShowData(cd);
            return dto;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private async Task<UsuarioNormalShowDataDTO> GetDatosParaShowData(CDUsuarioCompleto cd)
        {
            UsuarioNormalShowDataDTO objeto;
            List<PrendaAdminDTO> prendasDTO;
            List<ConjuntoAdminDTO> conjuntosDTO;
            Adjuntos adjuntos;
            DataRow fila;

            objeto = new UsuarioNormalShowDataDTO();
            objeto.Usuario = new UsuarioNormalDTO(cd.Usuario);
            objeto.Idiomas = this._servicioCombos.GetListaElementosCombo(cd.Idiomas, true, o => o.Id, o => o.Nombre);

            // Cargar imagen del usuario
            if (objeto.Usuario.Id > 0)
            {
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Usuario, objeto.Usuario.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    objeto.Usuario.FotoBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }
            }

            // Procesar prendas
            prendasDTO = new List<PrendaAdminDTO>();
            if (cd.TablaPrendas != null && cd.TablaPrendas.Rows.Count > 0)
            {
                foreach (DataRow row in cd.TablaPrendas.Rows)
                {
                    PrendaAdminDTO prendaDTO;
                    int iPrendaId;

                    prendaDTO = new PrendaAdminDTO();
                    iPrendaId = UtilsConversion.GetInt(row["Id"]) ?? 0;
                    prendaDTO.Id = iPrendaId;
                    prendaDTO.CategoriaNombre = UtilsConversion.GetString(row["CategoriaNombre"]);
                    prendaDTO.ColorNombre = UtilsConversion.GetString(row["ColorNombre"]);
                    prendaDTO.MarcaNombre = UtilsConversion.GetString(row["MarcaNombre"]);

                    // Cargar imagen de la prenda
                    adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, iPrendaId);
                    if (adjuntos != null && adjuntos.Any())
                    {
                        prendaDTO.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                    }

                    prendasDTO.Add(prendaDTO);
                }
            }
            objeto.Prendas = prendasDTO;

            // Procesar conjuntos
            conjuntosDTO = new List<ConjuntoAdminDTO>();
            if (cd.TablaConjuntos != null && cd.TablaConjuntos.Rows.Count > 0)
            {
                foreach (DataRow row in cd.TablaConjuntos.Rows)
                {
                    ConjuntoAdminDTO conjuntoDTO;
                    int iConjuntoId;

                    conjuntoDTO = new ConjuntoAdminDTO();
                    iConjuntoId = UtilsConversion.GetInt(row["Id"]) ?? 0;
                    conjuntoDTO.Id = iConjuntoId;
                    conjuntoDTO.Descripcion = UtilsConversion.GetString(row["Descripcion"]);
                    conjuntoDTO.EstilonNombre = UtilsConversion.GetString(row["EstiloNombre"]);
                    conjuntoDTO.EsFavorito = UtilsConversion.GetBool(row["EsFavorito"]);
                    conjuntoDTO.CantidadPrendas = UtilsConversion.GetInt(row["CantidadPrendas"]) ?? 0;

                    // Cargar imagen del conjunto
                    adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, iConjuntoId);
                    if (adjuntos != null && adjuntos.Any())
                    {
                        conjuntoDTO.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                    }

                    conjuntosDTO.Add(conjuntoDTO);
                }
            }
            objeto.Conjuntos = conjuntosDTO;

            return objeto;
        }

        #endregion
    }
}
