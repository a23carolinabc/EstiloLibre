using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.ContenedoresDatos;
using EstiloLibre_CapaNegocio.Servicios;
using static EstiloLibre_CapaNegocio.Consultas.ConsultasPrendas.Dtos;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasPrendas
    {
        #region ***** PROPIEDADES INTERNAS ***** 
        public Conexion _con;
        private ServicioCombos _servicioCombos;
        #endregion

        #region ***** CONSTRUCTOR ***** 
        public ConsultasPrendas(Conexion con, ServicioCombos servicioCombos)
        {
            this._con = con;
            this._servicioCombos = servicioCombos;
        }
        #endregion

        #region ***** MÉTODOS PÚBLICOS ***** 

        public PrendasAddNewDto GetDatosAddNew()
        {
            CDPrendasAddNew cd;
            PrendasAddNewDto dto;


            cd = new CDPrendasAddNew(this._con);
            cd.Cargar();

            dto = this.GetDatosParaAddNew(cd);
            return dto;
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        private PrendasAddNewDto GetDatosParaAddNew(CDPrendasAddNew cd)
        {
            PrendasAddNewDto objeto;

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

        #endregion
    }
}
