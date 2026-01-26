using TMPro;
using UnityEngine;

public class Recoger : MonoBehaviour
{
    private Player playerEnRango;
    public TextMeshProUGUI texto;

    public TextMeshProUGUI textoAumentar;
    public int puntos = 10;
    public int puntosAumentar = 10;
    public int aumentoDano = 10;
    public int ammo = 100;
    public TiposArmas tipoArma = TiposArmas.PISTOLA;

    public Transform posocionMango;

    public bool tieneArma=false; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerEnRango = other.GetComponent<Player>();
        if (playerEnRango == null) return;

        playerEnRango.SetObjetoRecogible(this);

        if (tieneArma == true)
        {
            tieneArma = true;

            string municion = LocalizationManager.Instance.GetTranslation("MUNICION");

            string puntosTexto = LocalizationManager.Instance.GetTranslation("PUNTOS");
            texto.text = $"{municion} {puntos} {puntosTexto}";
            textoAumentar.enabled = true;
            textoAumentar.text = $"{LocalizationManager.Instance.GetTranslation("AUMENTAR_DANO")} {puntosAumentar} {puntosTexto}";
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

        string puntosTexto = LocalizationManager.Instance.GetTranslation("PUNTOS");
        texto.text = $"{municion} {puntos} {puntosTexto}";
        textoAumentar.enabled = true;
        textoAumentar.text = $"{LocalizationManager.Instance.GetTranslation("AUMENTAR_DANO")} {puntosAumentar} {puntosTexto}";
        texto.enabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerEnRango.ClearObjetoRecogible(this);
        playerEnRango = null;
        texto.enabled = false;
        textoAumentar.enabled = false;
    }
}
