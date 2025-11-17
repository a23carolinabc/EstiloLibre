namespace EstiloLibre_CapaNegocio.Base
{
    public interface IDAO
    {
        ObjetoBD? CargarObjetoBD(int iId);
        ObjetoBD? CargarObjetoBD(string clausulaWhere, string? orderBy = null);
        void GuardarObjetoBD(ObjetoBD objetoBD);
        void EliminarObjetoBD(ObjetoBD objetoBD);
    }
}
