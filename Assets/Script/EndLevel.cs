using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    public static EndLevel Instance;

    public TextMeshProUGUI texto;
    public Image mira;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FinishGame(int points)
    {
        mira.enabled = false;

        string tienes = LocalizationManager.Instance.GetTranslation("TIENESPUNTOS");
        string puntos = LocalizationManager.Instance.GetTranslation("PUNTOS");

        texto.text = $"{tienes} {points} {puntos}";
        texto.enabled = true;
    }
}
