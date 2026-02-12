using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class EndLevel : MonoBehaviour
{
    // Singleton instance
    public static EndLevel Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI texto;
    public GameObject botonFinal;
    public GameObject mira;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        if (mira != null)
            mira.SetActive(true);

        if (botonFinal != null)
            botonFinal.gameObject.SetActive(false);
    }

    public void FinishGame(int points)
    {
        Time.timeScale = 0f;


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (mira != null)
            mira.SetActive(false);

        string tienes = LocalizationManager.Instance.GetTranslation("TIENESPUNTOS");
        string puntos = LocalizationManager.Instance.GetTranslation("PUNTOS");

        if (texto != null)
        {
            texto.text = $"{tienes} {points} {puntos}";
            texto.enabled = true;
        }

        if (botonFinal != null)
            botonFinal.gameObject.SetActive(true);
    }
}
