#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardSpawner))]
public class KeyboardSpawnerInspector : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        KeyboardSpawner KS = (KeyboardSpawner)target;

        if (GUILayout.Button("Spawn")) {
            KS.SpawnKeyboard();
        }

        if (GUILayout.Button("Clear")) {
            KS.ClearKeyboard();
        }
    }
}

[CustomEditor(typeof(KeyboardSpawnerCanvas))]
public class KeyboardSpawnerCanvasInspector : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        KeyboardSpawnerCanvas KS = (KeyboardSpawnerCanvas)target;

        if (GUILayout.Button("Spawn")) {
            KS.SpawnKeyboard();
        }

        if (GUILayout.Button("Clear")) {
            KS.ClearKeyboard();
        }
    }
}
#endif
