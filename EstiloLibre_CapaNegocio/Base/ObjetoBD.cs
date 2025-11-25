using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.Excepciones;
using System.Data;

namespace EstiloLibre_CapaNegocio.Base
{
    public abstract class ObjetoBD
    {
        #region ***** PROPIEDADES *****

        private int _id;

        [Write(false)]
        public IDAO? DAO { get; set; }

        [Write(false)]
        protected bool ExisteEnBD { get; set; }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; if (value > 0) ExisteEnBD = true; }
        }

        #endregion       

        #region ***** CONSTRUCTORES *****

        public ObjetoBD() 
            : base() 
        {
        }

        public ObjetoBD(IDAO dao) 
            : base()
        {
            this.DAO = dao;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public virtual void Guardar()
        {
            if (this.DAO == null)
            {
                throw new CapaNegocioException("No se ha establecido el DAO en el objeto '" + this.GetType().Name + "' con identificador " + this.Id);
            }
            this.DAO.GuardarObjetoBD(this);
            this.ExisteEnBD = true;
        }

        public virtual void Eliminar()
        {
            if (this.DAO == null)
            {
                throw new CapaNegocioException("No se ha establecido el DAO en el objeto '" + this.GetType().Name + "' con identificador " + this.Id);
            }
            this.DAO.EliminarObjetoBD(this);
            this.ExisteEnBD = false;
        }
        #endregion        
    }
}

