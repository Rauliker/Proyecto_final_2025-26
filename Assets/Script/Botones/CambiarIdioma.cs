using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CambiarIdioma : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private List<Idioma> idiomasDisponibles;

    private void Start()
    {
        LoadIdiomasDisponibles();
        SetupDropdown();
    }

    private void LoadIdiomasDisponibles()
    {
        idiomasDisponibles = new List<Idioma>
        {
            Idioma.Espanol,
            Idioma.Ingles
        };
    }

    private void SetupDropdown()
    {
        dropdown.ClearOptions();

        List<string> opciones = new List<string>();

        foreach (Idioma idioma in idiomasDisponibles)
        {
            // Mapeamos explícitamente los nombres a mostrar
            string nombreAMostrar = idioma == Idioma.Espanol ? "Español" : "English";
            opciones.Add(nombreAMostrar);
        }

        dropdown.AddOptions(opciones);

        // Seleccionar el idioma actual del LocalizationManager
        int index = idiomasDisponibles.IndexOf(LocalizationManager.Instance.idiomaActual);
        if (index >= 0)
            dropdown.value = index;

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        Idioma idiomaSeleccionado = idiomasDisponibles[index];

        LocalizationManager.Instance.SaveConfig(idiomaSeleccionado);

        Debug.Log("Idioma cambiado a: " + (idiomaSeleccionado == Idioma.Espanol ? "Espanol" : "English"));
    }
}
