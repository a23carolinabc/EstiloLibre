using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Servicios
{
    public class ServicioCombos
    {
        public IEnumerable<ControlItem> GetListaElementosCombo<TObjeto, TValor>(IEnumerable<TObjeto> listaObjetos,
                    bool bAnadirElementoVacio, Func<TObjeto, TValor> fnGetValor, Func<TObjeto, string> fnGetTexto = null)
        {
            TValor valor;
            ControlItem item;
            List<ControlItem> lstResultado;

            lstResultado = new List<ControlItem>();
            if ((listaObjetos != null))
            {
                foreach (TObjeto objeto in listaObjetos)
                {
                    valor = fnGetValor(objeto);
                    if ((fnGetTexto == null))
                        item = new ControlItem(objeto.ToString(), valor);
                    else
                        item = new ControlItem(fnGetTexto(objeto), valor);
                    lstResultado.Add(item);
                }
            }
            return lstResultado;
        }
    }
}
