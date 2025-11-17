using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs;

internal class IdiomasDAO : DAO<Idioma>
{
    #region ***** CONSTRUCTORES *****    
    public IdiomasDAO(Conexion conexion)
        :base(conexion, TablasBD.Idiomas) 
    { 
    }

    #endregion

    #region ***** MÉTODOS PÚBLICOS *****
    public override ObjetoBD CrearObjetoBD()
    {
        return new Idioma(this);
    }
    #endregion
}
