namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public class Codigos
    {
        internal class Idiomas
        {
            public const string Español = "es";
            public const string Gallego = "gl";
        }

        internal class Permisos
        {
            public const string LEC_Usuarios = "LEC_Usuarios";
            public const string MOD_Usuarios = "MOD_Usuarios";
            public const string LEC_Prendas = "LEC_Prendas";
            public const string MOD_Prendas = "MOD_Prendas";
            public const string ADMIN = "ADMIN";
            public const string USER = "USER";
        }

        internal class ClasesObjetos
        {
            public const int Prenda = 1;
            public const int Conjunto = 2;
        }

        internal class TiposAdjuntos
        {
            public const int Imagen = 1;
        }
    }
}
