using TMPro;
using UnityEngine;

public class CambiarTexto : MonoBehaviour
{

    public string texto;

    void Start()
    {
        string resultado = texto.ToUpper().Replace(" ", "");
        

        TextMeshProUGUI textoTMP = transform.GetComponent<TextMeshProUGUI>();
        if (textoTMP == null)
        {
            Debug.LogError("No se encontró un componente TextMeshProUGUI en 'Text (TMP)'");
            return;
        }

        string recoger = LocalizationManager.Instance.GetTranslation(resultado);
        textoTMP.text = recoger;
        textoTMP.enabled = true;
    }
}
