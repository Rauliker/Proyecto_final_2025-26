using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TiposDeBotones
{
    Inicio,
    Configuracion,
    Salir
}

public class Botones : MonoBehaviour
{
    [Header("Tipo de Botón")]
    public TiposDeBotones TipodeBoton;

    [Header("Opciones de Escena")]
    public string nombreNivel;

    [Header("Objetos UI")]
    public List<UIElementEntry> elementsUI;

    void Start()
    {
        AsignarFuncion();
    }

    void AsignarFuncion()
    {
        Button boton = GetComponent<Button>();

        if (!boton)
        {
            Debug.LogError("No se encontró un componente Button.");
            return;
        }

        boton.onClick.RemoveAllListeners();

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
        }
    }

    public void Inicio()
    {
        if (string.IsNullOrEmpty(nombreNivel)) return;

        if (Application.isPlaying && SceneManager.GetSceneByName(nombreNivel) != null)
        {
            SceneManager.LoadScene(nombreNivel);
        }
        else
        {
            Debug.LogError($"La escena '{nombreNivel}' no existe en Build Settings.");
        }
    }

    public void Configuracion()
    {
        foreach (var entry in elementsUI)
        {
            Debug.Log($"Activando: {entry.value.name} -> {entry.key}");

            entry.value.SetActive(entry.key);
        }
    }

    public void Salir()
    {
        Debug.Log("Botón Salir pulsado");
        Application.Quit();
    }
}
