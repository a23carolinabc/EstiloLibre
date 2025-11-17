using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class UsuariosDAO : DAO<Usuario>
    {
        #region ***** CONSTRUCTORES *****
        public UsuariosDAO(Conexion conexion) : base(conexion, TablasBD.Usuarios) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new Usuario(this);
        }
        public Usuario? CargarUsuario(int usuarioId)
        {
            return (Usuario?)this.CargarObjetoBD(usuarioId);
        }
        public Usuario? CargarUsuarioPorLogin(string login)
        {
            return (Usuario?)this.CargarObjetoBD($"Login = '{login}'");
        }
        #endregion
    }    
}
