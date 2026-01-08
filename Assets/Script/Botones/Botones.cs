using TMPro;
using UnityEngine;

[System.Serializable]
public enum TiposDeBotones
{
    Inicio,
    Configuracion,
    Salir
}

public class Botones : MonoBehaviour
{
    public string texto;
    public TiposDeBotones TipodeBoton;
    void Start()
    {
        Cambiar_texto();
    }

    public void Cambiar_texto()
    {
        string resultado = texto.ToUpper().Replace(" ", "");

        Transform textTransform = transform.Find("Text (TMP)");
        TextMeshProUGUI textoTMP = textTransform.GetComponent<TextMeshProUGUI>();
        string recoger = LocalizationManager.Instance.GetTranslation(resultado);

        textoTMP.text = recoger;
        textoTMP.enabled = true;
    }

    public void Inicio()
    {
        Debug.Log("Botón pulsado");
    }

    public void Configuracion()
    {
        Debug.Log("Botón pulsado");
    }

    public void Salir()
    {
        Debug.Log("Botón pulsado");
    }
}
