using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class EstilosDAO : DAO<Estilo>
    {
        #region ***** CONSTRUCTORES *****
        public EstilosDAO(Conexion conexion) : base(conexion, TablasBD.Estilos) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new Estilo(this);
        }
        public Estilo? CargarEstilo(int iEstiloId)
        {
            return (Estilo?)this.CargarObjetoBD(iEstiloId);
        }
        #endregion
    }    
}
