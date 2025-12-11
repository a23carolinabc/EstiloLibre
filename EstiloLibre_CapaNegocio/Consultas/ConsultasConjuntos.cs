using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasConjuntos.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasConjuntos
    {
        #region ***** PROPIEDADES *****

        private readonly Conexion _con;
        private readonly ServicioCombos _servicioCombos;
        private readonly ServicioAlmacenamiento _servicioAlmacenamiento;

        #endregion

        #region ***** CONSTRUCTORES *****

        public ConsultasConjuntos(Conexion con)
        {
            this._con = con;
            this._servicioCombos = new ServicioCombos();
            this._servicioAlmacenamiento = new ServicioAlmacenamiento(con.ConfiguracionEstiloLibre);
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public async Task<ConjuntosAddNewDTO> GetDatosAddNew()
        {
            CDConjuntosAddNew cd;
            ConjuntosAddNewDTO objeto;

            cd = new CDConjuntosAddNew(this._con);
            cd.Cargar();

            objeto = new();
            objeto.Estaciones = this._servicioCombos.GetListaElementosCombo(cd.Estaciones, true, o => o.Id, o => o.Nombre);
            objeto.Estilos = this._servicioCombos.GetListaElementosCombo(cd.Estilos, true, o => o.Id, o => o.Nombre);
            objeto.Colores = this._servicioCombos.GetListaElementosCombo(cd.Colores, true, o => o.Id, o => o.Nombre);
            
            return await Task.FromResult(objeto);
        }

        public async Task<ConjuntosShowDataDTO> GetDatosShowData(int iConjuntoId)
        {
            CDConjuntosShowData cd;
            ConjuntosShowDataDTO objeto;
            Adjuntos adjuntos;
            List<int> prendasIds;

            cd = new CDConjuntosShowData(this._con);
            cd.Cargar(iConjuntoId);

            objeto = new();
            objeto.Estaciones = this._servicioCombos.GetListaElementosCombo(cd.Estaciones, true, o => o.Id, o => o.Nombre);
            objeto.Estilos = this._servicioCombos.GetListaElementosCombo(cd.Estilos, true, o => o.Id, o => o.Nombre);
            objeto.Colores = this._servicioCombos.GetListaElementosCombo(cd.Colores, true, o => o.Id, o => o.Nombre);
            objeto.Conjunto = new ConjuntoDTO(cd.Conjunto);

            // Cargar imagen del conjunto si existe
            if (objeto.Conjunto.Id > 0)
            {
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, objeto.Conjunto.Id);
                if (adjuntos != null && adjuntos.Any())
                {
                    objeto.Conjunto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                // Cargar IDs de prendas asociadas
                prendasIds = cd.PrendasConjuntos.Select(cp => cp.PrendaId).ToList();
                objeto.Conjunto.PrendasIds = prendasIds;
            }

            return objeto;
        }

        public async Task<IEnumerable<ConjuntoDTO>> GetConjuntosUsuario(int iUsuarioId)
        {
            Conjuntos conjuntos;
            List<ConjuntoDTO> lista;
            Adjuntos adjuntos;

            conjuntos = this._con.CargarConjuntos(iUsuarioId);

            lista = new List<ConjuntoDTO>();

            foreach (Conjunto conjunto in conjuntos)
            {
                ConjuntoDTO dto;

                dto = new ConjuntoDTO(conjunto);

                // Cargar imagen miniatura
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Conjunto, conjunto.Id);
                if (adjuntos != null && adjuntos.Count > 0)
                {
                    dto.ImagenBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }

                lista.Add(dto);
            }

            return lista;
        }        

        #endregion
    }
}