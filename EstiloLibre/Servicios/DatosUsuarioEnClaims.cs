namespace EstiloLibre.Servicios;

public class DatosUsuarioEnClaims
{
    public List<string> _lstPermisosAcceso;

    public bool EsPersona { get; set; }
    public string DireccionIPLogin { get; set; }
    public string Login { get; set; }
    public string NombrePersona { get; set; }
    public string Apellidos { get; set; }
    public int UsuarioId { get; set; }
    public int IdiomaId { get; set; }
    public string CodigoIdiomaActual { get; set; }
    public IEnumerable<string> ListaPermisosAcceso
    {
        get { return this._lstPermisosAcceso; }
    }

    public DatosUsuarioEnClaims()
    {
        this._lstPermisosAcceso = new List<string>();
    }

    public void AñadirPermisoAcceso(string strCodigo)
    {
        this._lstPermisosAcceso.Add(strCodigo);
    }
}
