using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Servicios;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuarios
    {
        #region ***** PROPIEDADES INTERNAS ***** 
        public Conexion _con;
        public UsuariosDAO _dao;
        private ServicioCombos _servicioCombos;
        #endregion

        #region ***** CONSTRUCTOR ***** 
        public ConsultasUsuarios(Conexion con, ServicioCombos servicioCombos)
        {
            this._con = con;
            this._dao = new UsuariosDAO(_con);
            this._servicioCombos = servicioCombos;
        }
        #endregion

        #region ***** MÉTODOS PRIVADOS ***** 

        public Usuario? GetUsuario(int usuarioId)
        {
            return this._dao.CargarUsuario(usuarioId);
        }

        public Usuario? GetUsuarioPorLogin(string login)
        {
            return this._dao.CargarUsuarioPorLogin(login);
        }

        #endregion
    }
}
