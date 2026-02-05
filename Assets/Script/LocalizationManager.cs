using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public Idioma idiomaActual;

    public string csvFileName = "localization";

    private Dictionary<string, string[]> traducciones;

    private Dictionary<string, string> variables = new Dictionary<string, string>();

    private Regex tokenRegex = new Regex(@"\$[a-zA-Z0-9_\.]+");

    public BotonesConfig botones;

    public event Action OnLanguageChanged;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            ApllyConfig();         // Lee config.json
            LoadJson();           // Botones
            LoadCSV();
            LoadBotonesVariables();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ConfigData LoadConfig()
    {
        string path = Path.Combine(Application.persistentDataPath, "config.json");

        // Si no existe, copiar desde Resources
        if (!File.Exists(path))
        {
            TextAsset defaultConfig = Resources.Load<TextAsset>("config");
            File.WriteAllText(path, defaultConfig.text);
        }

        string jsonText = File.ReadAllText(path);
        return JsonUtility.FromJson<ConfigData>(jsonText);
    }


    // Carga config.json
    private void ApllyConfig()
    {
        ConfigData configData = LoadConfig();
        if (configData.Idioma == "Espanol")
            idiomaActual = (Idioma)Enum.Parse(typeof(Idioma), configData.Idioma, true);
        else if(configData.Idioma == "Ingles")
            idiomaActual = Idioma.Ingles; 

    }

    // Función para editar config.json
    public void SaveConfig(Idioma nuevoIdioma)
    {
        ConfigData configData = new ConfigData
        {
            Idioma = nuevoIdioma == Idioma.Espanol ? "Espanol" : "Ingles"
        };

        string path = Path.Combine(Application.persistentDataPath, "config.json");
        File.WriteAllText(path, JsonUtility.ToJson(configData, true));

        idiomaActual = nuevoIdioma;

        Debug.Log("Idioma actualizado en tiempo real: " + configData.Idioma);

        OnLanguageChanged?.Invoke();
    }



    private void LoadJson()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("botones");
        if (jsonText != null)
        {
            botones = JsonUtility.FromJson<BotonesConfig>(jsonText.text);
            Debug.Log("Botones cargados correctamente");
        }
        else
        {
            Debug.LogError("No se pudo cargar el archivo botones.json");
        }
    }

    private void LoadBotonesVariables()
    {
        if (botones == null) return;

        FieldInfo[] fields = typeof(BotonesConfig).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            string key = "boton." + field.Name.ToLower();
            string value = field.GetValue(botones)?.ToString() ?? "";
            variables[key] = value;
        }
    }

    private void LoadCSV()
    {
        traducciones = new Dictionary<string, string[]>();

        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);
        if (csvFile == null)
        {
            Debug.LogError("No se encontró el archivo CSV: " + csvFileName);
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        foreach (string lineRaw in lines)
        {
            string line = lineRaw.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            if (values.Length < 3) continue;

            string key = values[0].Trim();

            traducciones[key] = values;

        }

        Debug.Log("CSV cargado. Traducciones: " + traducciones.Count);
    }
    public float GetVolumen(string tipo)
    {
        ConfigData config = LoadConfig();
        return tipo == "Musica" ? config.Musica : config.SFX;
    }

    public void SaveVolumen(string tipo, float valor)
    {
        ConfigData config = LoadConfig();

        if (tipo == "Musica")
            config.Musica = valor;
        else
            config.SFX = valor;

        string path = Path.Combine(Application.persistentDataPath, "config.json");
        File.WriteAllText(path, JsonUtility.ToJson(config, true));
    }
    public string GetTranslation(string key)
    {
        return GetTranslationRecursive(key, new HashSet<string>());
    }

    private string GetTranslationRecursive(string key, HashSet<string> visitedKeys)
    {
        if (!traducciones.ContainsKey(key) && !variables.ContainsKey(key))
            return key;

        if (visitedKeys.Contains(key))
            return key;

        visitedKeys.Add(key);

        string texto;

        if (traducciones.ContainsKey(key))
        {
            string[] row = traducciones[key];
            int idiomaIndex = idiomaActual == Idioma.Espanol ? 1 : 2;
            if (idiomaIndex >= row.Length)
                return key;

            texto = row[idiomaIndex].Trim();
        }
        else
        {
            texto = variables[key];
        }

        MatchCollection matches = tokenRegex.Matches(texto);
        foreach (Match match in matches)
        {
            string token = match.Value.Substring(1);
            string tokenTranslation = GetTranslationRecursive(token, visitedKeys);
            texto = texto.Replace(match.Value, tokenTranslation);
        }

        return texto;
    }


    [System.Serializable]
    public class ConfigData
    {
        public string Idioma;
        public float Musica = 1f;
        public float SFX = 1f;
    }

}
