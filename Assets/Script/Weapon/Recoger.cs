using TMPro;
using UnityEngine;

public class Recoger : MonoBehaviour
{
    private Player playerEnRango;
    public TextMeshProUGUI texto;

    public TiposArmas tipoArma=TiposArmas.PISTOLA;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerEnRango = other.GetComponent<Player>();

        if (playerEnRango == null)
        {
            Debug.LogError("El objeto Player no tiene el componente Player");
            return;
        }

        playerEnRango.SetObjetoRecogible(this);

        if (texto == null)
        {
            Debug.LogError("Texto no asignado en el Inspector");
            return;
        }

        if (LocalizationManager.Instance == null)
        {
            Debug.LogError("LocalizationManager.Instance es NULL");
            return;
        }

        string recoger = LocalizationManager.Instance.GetTranslation("RECOGER_ARMA");
        string arma = LocalizationManager.Instance.GetTranslation(tipoArma.ToString());

        texto.text = $"{recoger} {arma}";
        texto.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        DissableText(other);
    }

    public void DissableText(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnRango.ClearObjetoRecogible(this);
            playerEnRango = null;
            texto.enabled = false;
        }
    }
}
