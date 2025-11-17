using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos;

public class Idioma : ObjetoBD
{
    #region ***** PROPIEDADES *****

    public string Codigo { get; set; }

    public string Nombre { get; set; }

    #endregion

    #region ***** CONSTRUCTORES *****

    public Idioma() : base() { }

    public Idioma(DAO<Idioma> dao) : base(dao) { }

    #endregion
}
