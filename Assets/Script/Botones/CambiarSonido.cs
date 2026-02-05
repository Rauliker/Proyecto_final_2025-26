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
        slider.onValueChanged.AddListener(CambiarVolumen);
    }

    void Start()
    {
        float volumenGuardado = LocalizationManager.Instance.GetVolumen(parametroMixer);

        slider.SetValueWithoutNotify(volumenGuardado);
        AplicarVolumen(volumenGuardado);
    }

    void CambiarVolumen(float valor)
    {
        AplicarVolumen(valor);

        // Guardar en config.json
        LocalizationManager.Instance.SaveVolumen(parametroMixer, valor);
    }

    void AplicarVolumen(float valor)
    {
        if (valor <= 0f)
            mixer.SetFloat(parametroMixer, -80f);
        else
            mixer.SetFloat(parametroMixer, Mathf.Log10(valor) * 20);
    }
}
