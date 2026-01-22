using TMPro;
using UnityEngine;

public class Recoger : MonoBehaviour
{
    private Player playerEnRango;
    public TextMeshProUGUI texto;
    public int puntos = 10;
    public int ammo = 10;
    public TiposArmas tipoArma = TiposArmas.PISTOLA;

    public bool tieneArma=false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerEnRango = other.GetComponent<Player>();
        if (playerEnRango == null) return;

        playerEnRango.SetObjetoRecogible(this);

        if (tieneArma == true)
        {
            string municion = LocalizationManager.Instance.GetTranslation("MUNICION");

            string textopuntos = LocalizationManager.Instance.GetTranslation("PUNTOS");
            texto.text = $"{municion} {puntos} {textopuntos}";
        } else
        {
            string recoger = LocalizationManager.Instance.GetTranslation("RECOGER_ARMA");
            string armaTexto = LocalizationManager.Instance.GetTranslation(tipoArma.ToString());
            texto.text = $"{recoger} {armaTexto}";
        }
        texto.enabled = true;
    }


    public void ActualizarTieneArma()
    {
        tieneArma = true;

        string municion = LocalizationManager.Instance.GetTranslation("MUNICION");

        string puntos = LocalizationManager.Instance.GetTranslation("PUNTOS");
        texto.text = $"{municion} {puntos} {puntos}";

        texto.enabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerEnRango.ClearObjetoRecogible(this);
        playerEnRango = null;
        texto.enabled = false;
    }
}
