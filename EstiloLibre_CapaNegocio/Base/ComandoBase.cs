namespace EstiloLibre_CapaNegocio.Base;

public class ComandoBase
{
    #region ***** PROPIEDADES *****

    internal string Nombre { get; private set; }
    internal string[] CodigosPermisos { get; private set; }

    #endregion

    #region ***** CONSTRUCTORES *****

    protected ComandoBase(string strNombre, params string[] aCodigosPermsios)
    {
        this.Nombre = strNombre;
        this.CodigosPermisos = aCodigosPermsios;
    }

    protected ComandoBase(params string[] aCodigosPermsios)
    {
        this.Nombre = this.GetType().Name;
        this.CodigosPermisos = aCodigosPermsios;
    }

    #endregion
}
