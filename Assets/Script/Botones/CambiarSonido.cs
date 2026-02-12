using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CambiarSonido : MonoBehaviour
{
    public AudioMixer mixer;
    public string parametroMixer; 

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        // Obtener el volumen guardado (0 a 1)
        float volumenGuardado = LocalizationManager.Instance.GetVolumen(parametroMixer);

        // Ajustar el slider sin disparar OnValueChanged
        slider.SetValueWithoutNotify(volumenGuardado);

        // Aplicar volumen al AudioMixer
        AplicarVolumen(volumenGuardado);
    }

    public void CambiarVolumen(float valor)
    {
        AplicarVolumen(valor);
        LocalizationManager.Instance.SaveVolumen(parametroMixer, valor);
    }

    private void AplicarVolumen(float valor)
    {
        if (valor <= 0f)
             mixer.SetFloat(parametroMixer, -80f); // Silencio
        else
            mixer.SetFloat(parametroMixer, Mathf.Log10(valor) * 20f); // Escala logarítmica
    }
}
