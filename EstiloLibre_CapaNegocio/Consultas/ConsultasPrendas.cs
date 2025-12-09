using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasPrendas.DTOs;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasPrendas
    {
        #region ***** PROPIEDADES INTERNAS ***** 
        public Conexion _con;
        private ServicioCombos _servicioCombos;
        private ServicioAlmacenamiento _servicioAlmacenamiento;
        #endregion

        #region ***** CONSTRUCTOR ***** 
        public ConsultasPrendas(Conexion con, ServicioCombos servicioCombos, ServicioAlmacenamiento servicioAlmacenamiento)
        {
            this._con = con;
            this._servicioCombos = servicioCombos;
            this._servicioAlmacenamiento = servicioAlmacenamiento;
        }
        #endregion

        #region ***** MÉTODOS PÚBLICOS ***** 

        public PrendasAddNewDTO GetDatosAddNew()
        {
            CDPrendasAddNew cd;
            PrendasAddNewDTO dto;


            cd = new CDPrendasAddNew(this._con);
            cd.Cargar();

            dto = this.GetDatosParaAddNew(cd);
            return dto;
        }

        public async Task<PrendasShowDataDTO> GetDatosShowData(int iPrendaId)
        {
            CDPrendasShowData cd;
            PrendasShowDataDTO dto;


            cd = new CDPrendasShowData(this._con);
            cd.Cargar(iPrendaId);

            dto = await this.GetDatosParaShowData(cd);
            return dto;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private PrendasAddNewDTO GetDatosParaAddNew(CDPrendasAddNew cd)
        {
            PrendasAddNewDTO objeto;

            objeto = new();
            objeto.Marcas = this._servicioCombos.GetListaElementosCombo(cd.Marcas, true, o => o.Id, o => o.Nombre);
            objeto.Estaciones = this._servicioCombos.GetListaElementosCombo(cd.Estaciones, true, o => o.Id, o => o.Nombre);
            objeto.Tallas = this._servicioCombos.GetListaElementosCombo(cd.Tallas, true, o => o.Id, o => o.Nombre);
            objeto.Materiales = this._servicioCombos.GetListaElementosCombo(cd.Materiales, true, o => o.Id, o => o.Nombre);
            objeto.Colores = this._servicioCombos.GetListaElementosCombo(cd.Colores, true, o => o.Id, o => o.Nombre);
            objeto.Categorias = this._servicioCombos.GetListaElementosCombo(cd.Categorias, true, o => o.Id, o => o.Nombre);
            objeto.Estados = this._servicioCombos.GetListaElementosCombo(cd.Estados, true, o => o.Id, o => o.Nombre);

            return objeto;
        }

        private async Task<PrendasShowDataDTO> GetDatosParaShowData(CDPrendasShowData cd)
        {
            PrendasShowDataDTO objeto;
            Adjuntos adjuntos;

            objeto = new();            
            objeto.Marcas = this._servicioCombos.GetListaElementosCombo(cd.Marcas, true, o => o.Id, o => o.Nombre);
            objeto.Estaciones = this._servicioCombos.GetListaElementosCombo(cd.Estaciones, true, o => o.Id, o => o.Nombre);
            objeto.Tallas = this._servicioCombos.GetListaElementosCombo(cd.Tallas, true, o => o.Id, o => o.Nombre);
            objeto.Materiales = this._servicioCombos.GetListaElementosCombo(cd.Materiales, true, o => o.Id, o => o.Nombre);
            objeto.Colores = this._servicioCombos.GetListaElementosCombo(cd.Colores, true, o => o.Id, o => o.Nombre);
            objeto.Categorias = this._servicioCombos.GetListaElementosCombo(cd.Categorias, true, o => o.Id, o => o.Nombre);
            objeto.Estados = this._servicioCombos.GetListaElementosCombo(cd.Estados, true, o => o.Id, o => o.Nombre);
            objeto.Prenda = new(cd.Prenda);
            if(objeto.Prenda.Id > 0)
            {
                adjuntos = this._con.CargarAdjuntos(Codigos.ClasesObjetos.Prenda, objeto.Prenda.Id);
                if(adjuntos != null)
                {
                    objeto.Prenda.FotoBase64 = await this._servicioAlmacenamiento.ObtenerImagenBase64(adjuntos.First());
                }
            }
            
            return objeto;
        }

        #endregion
    }
}
