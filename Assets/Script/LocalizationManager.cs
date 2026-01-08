using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Reflection;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public Idioma idiomaActual = Idioma.Espanol;

    public string csvFileName = "localization";

    private Dictionary<string, string[]> traducciones;

    private Dictionary<string, string> variables = new Dictionary<string, string>();

    private Regex tokenRegex = new Regex(@"\$[a-zA-Z0-9_\.]+");

    public BotonesConfig botones;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadJson();
            LoadCSV();
            LoadBotonesVariables(); 
        }
        else
        {
            Destroy(gameObject);
        }
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

        // Usamos reflection para recorrer todos los campos públicos de BotonesConfig
        FieldInfo[] fields = typeof(BotonesConfig).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            string key = "boton." + field.Name.ToLower(); // ejemplo: boton.recoger
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
            // Variable dinámica
            texto = variables[key];
        }

        // Reemplazo de tokens recursivo
        MatchCollection matches = tokenRegex.Matches(texto);
        foreach (Match match in matches)
        {
            string token = match.Value.Substring(1); // quitamos $

            string tokenTranslation = GetTranslationRecursive(token, visitedKeys);
            texto = texto.Replace(match.Value, tokenTranslation);
        }

        return texto;
    }
}
