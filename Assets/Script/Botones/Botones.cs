using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para Button
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    public bool pasarNivel;
    public int indiceNivel;

    void Start()
    {
        Cambiar_texto();
        AsignarFuncion();
    }

    void AsignarFuncion()
    {
        // Obtener el componente Button del objeto
        Button boton = GetComponent<Button>();
        if (boton == null)
        {
            Debug.LogError("No se encontró un componente Button en este GameObject");
            return;
        }

        // Limpiar listeners anteriores para evitar duplicados
        boton.onClick.RemoveAllListeners();

        // Asignar la función correcta según el tipo de botón
        switch (TipodeBoton)
        {
            case TiposDeBotones.Inicio:
                boton.onClick.AddListener(Inicio);
                break;
            case TiposDeBotones.Configuracion:
                boton.onClick.AddListener(Configuracion);
                break;
            case TiposDeBotones.Salir:
                boton.onClick.AddListener(Salir);
                break;
            default:
                Debug.LogError("Error en el switch de botones");
                break;
        }
    }

    public void Cambiar_texto()
    {
        string resultado = texto.ToUpper().Replace(" ", "");

        Transform textTransform = transform.Find("Text (TMP)");
        if (textTransform == null)
        {
            Debug.LogError("No se encontró el hijo 'Text (TMP)'");
            return;
        }

        TextMeshProUGUI textoTMP = textTransform.GetComponent<TextMeshProUGUI>();
        if (textoTMP == null)
        {
            Debug.LogError("No se encontró un componente TextMeshProUGUI en 'Text (TMP)'");
            return;
        }

        string recoger = LocalizationManager.Instance.GetTranslation(resultado);
        textoTMP.text = recoger;
        textoTMP.enabled = true;
    }

    public void Inicio()
    {
        SceneManager.LoadScene(indiceNivel);
    }

    public void Configuracion()
    {
        Debug.Log("Botón Configuración pulsado");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
