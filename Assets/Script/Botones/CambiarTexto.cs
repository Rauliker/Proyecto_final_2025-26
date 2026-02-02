using UnityEngine;
using TMPro;
using System.Collections;

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
    private IEnumerator Start()
    {
        while (LocalizationManager.Instance == null)
            yield return null;

        RefreshText();
        LocalizationManager.Instance.OnLanguageChanged += RefreshText;
    }



    private void OnEnable()
    {
        // Suscribirse al evento de cambio de idioma
        RefreshText();
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged += RefreshText;

        
    }

    private void OnDisable()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= RefreshText;
    }

    public void RefreshText()
    {
        if (LocalizationManager.Instance == null)
            return;

        if (textoTMP == null)
            textoTMP = GetComponent<TextMeshProUGUI>();

        string key = texto.ToUpper().Replace(" ", "");
        textoTMP.text = LocalizationManager.Instance.GetTranslation(key);
    }

}
