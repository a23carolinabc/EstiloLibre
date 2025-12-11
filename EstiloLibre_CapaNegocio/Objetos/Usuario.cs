using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;
using System.Data;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Usuarios)]
    public class Usuario : ObjetoBD
    {
        #region ****** PROPIEDADES *****
        
        public string Login { get; set; }
        public string Contraseña { get; set; }
        public int IdiomaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CorreoE { get; set; }
        public int Telefono { get; set; }
        public bool Publico { get; set; }
        [Write(false)]
        public List<string> Permisos { get; set; }
        
        #endregion

        #region ***** CONSTRUCTORES *****

        public Usuario() : base() { }

        public Usuario(UsuariosDAO dao) : base(dao) { }        
        
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****
        public void IniciarListaPermisos()
        {
            this.Permisos = new List<string>();
        }        
        #endregion
    }
}
