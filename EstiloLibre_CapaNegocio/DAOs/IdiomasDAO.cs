using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs;

public class IdiomasDAO : DAO<Idioma>
{
    #region ***** CONSTRUCTORES *****    
    public IdiomasDAO(Conexion conexion) :base(conexion, TablasBD.Idiomas) { }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****
    public override ObjetoBD CrearObjetoBD()
    {
        return new Idioma(this);
    }
    public Idioma? CargarIdioma(int iIdiomaId)
    {
        return (Idioma?)this.CargarObjetoBD(iIdiomaId);
    }
    public Idioma? CargarIdiomaPorLogin(string strCodigo)
    {
        return (Idioma?)this.CargarObjetoBD($"Codigo = '{strCodigo}'");
    }
    #endregion
}
