using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Botones))]
public class BotonesEditor : Editor
{
    SerializedProperty tipoBoton;
    SerializedProperty nombreNivel;
    SerializedProperty elementsUI;

    void OnEnable()
    {
        tipoBoton = serializedObject.FindProperty("TipodeBoton");
        nombreNivel = serializedObject.FindProperty("nombreNivel");
        elementsUI = serializedObject.FindProperty("elementsUI");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(tipoBoton);
        EditorGUILayout.Space();

        TiposDeBotones tipo = (TiposDeBotones)tipoBoton.enumValueIndex;

        if (tipo == TiposDeBotones.Inicio)
        {
            EditorGUILayout.PropertyField(nombreNivel);
            EditorGUILayout.HelpBox(
                "Este botón cargará la escena indicada.\nAsegúrate de que esté en Build Settings.",
                MessageType.Info
            );
        }

        if (tipo == TiposDeBotones.Configuracion)
        {
            EditorGUILayout.PropertyField(elementsUI, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
