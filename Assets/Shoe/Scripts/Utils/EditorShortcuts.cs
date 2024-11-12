#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class EditorShortcuts : EditorWindow
{
    [MenuItem("Window/Shoe/Editor Shortcuts")]
    public static void ShowWindow()
    {
        GetWindow<EditorShortcuts>("EditorShortcuts");
    }

    void OnGUI()
    {
        DrawShortcutButtons();
    }

    private void DrawShortcutButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Win"))
        {
            Services.Get<IGameManager>().CallWinState();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Lose"))
        {
            Services.Get<IGameManager>().CallLoseState();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Restart"))
        {
            Services.Get<IGameManager>().CallRestart();
        }
        EditorGUILayout.EndHorizontal();

    }
}
#endif