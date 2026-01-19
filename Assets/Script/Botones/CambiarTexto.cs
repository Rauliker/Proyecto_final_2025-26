using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedTextTMP : MonoBehaviour
{
    [Tooltip("Texto base para generar la key en el CSV")]
    public string texto;

    private TextMeshProUGUI textoTMP;

    private void Awake()
    {
        textoTMP = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Suscribirse al evento de cambio de idioma
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged += RefreshText;

        RefreshText();
    }

    private void OnDisable()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= RefreshText;
    }

    public void RefreshText()
    {
        if (textoTMP == null) return;

        // Generar la key del CSV
        string key = texto.ToUpper().Replace(" ", "");

        // Obtener la traducción
        string traducido = LocalizationManager.Instance.GetTranslation(key);

        // Asignar al TextMeshProUGUI
        textoTMP.text = traducido;
        textoTMP.enabled = true;
    }
}
