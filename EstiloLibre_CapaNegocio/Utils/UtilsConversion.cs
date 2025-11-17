using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstiloLibre_CapaNegocio.Utils
{
    public static class UtilsConversion
    {

        public static bool EsNulo(object? dato)
        {
            if (dato == null) return true;
            if (dato == DBNull.Value) return true;
            return false;
        }

        public static string? GetString(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;
            return Convert.ToString(dato);
        }

        public static int? GetInt(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;
            if (int.TryParse(GetString(dato), out var val)) return val;
            return null;
        }

        public static decimal? GetDecimal(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;
            if (decimal.TryParse(GetString(dato), out decimal val)) return val;
            return null;
        }

        public static double? GetDouble(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;
            if (double.TryParse(GetString(dato), out double val)) return val;
            return null;
        }

        public static bool GetBool(object? dato)
        {
            if (dato == null) return false;
            if (dato == DBNull.Value) return false;
            if (dato is int || dato is long || dato is short || dato is byte)
            {
                return Convert.ToInt32(dato) != 0;
            }

            if (dato is string texto)
            {
                texto = texto.Trim().ToLower();
                if (texto == "1" || texto == "true" || texto == "sí" || texto == "si" || texto == "yes" || texto == "s" || texto == "y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if(bool.TryParse(GetString(dato),out bool val)) return val;
            return false;
        }

        public static DateTime? GetDateTime(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;

            if (DateTime.TryParse(GetString(dato),out DateTime val)) return val;
            return null;
        }

        public static TimeSpan? GetTime(object? dato)
        {
            if (dato == null) return null;
            if (dato == DBNull.Value) return null;

            if (TimeSpan.TryParse(GetString(dato), out TimeSpan val)) return val;
            return null;
        }
    }
}
