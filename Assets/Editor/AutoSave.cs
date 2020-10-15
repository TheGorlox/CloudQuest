using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSave : EditorWindow {
    private DateTime nextSave;
    private float delay;
    private bool logMessage, delayEnabled, autoFocus;

    [MenuItem("Window/Auto Saver")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        AutoSave window = (AutoSave)EditorWindow.GetWindow(typeof(AutoSave));
        window.Show();
    }

    void Awake() {
        nextSave = DateTime.Now.AddSeconds((double)delay);
    }

    void OnInspectorUpdate() {
        if (DateTime.Now > nextSave || !delayEnabled) {
            nextSave = DateTime.Now.AddSeconds((double)delay);

            //save all scenes
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                if (SceneManager.GetSceneAt(i).isDirty) {
                    Scene sceneAtI = SceneManager.GetSceneAt(i);
                    EditorSceneManager.SaveScene(sceneAtI);
                    if (logMessage) {
                        Debug.LogWarning("Scene \"" + sceneAtI.name + "\" Saved!");
                    }
                }
            }
        }
    }

    void OnLostFocus() {
        if (autoFocus) {
            FocusWindowIfItsOpen<AutoSave>();
        }
    }

    void OnGUI() {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        delayEnabled = EditorGUILayout.BeginToggleGroup("Enable save delay", delayEnabled);
        EditorGUI.indentLevel = 1;
        delay = EditorGUILayout.Slider("Save Delay", delay, 1, 60);
        EditorGUI.indentLevel = 0;
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(10);
        logMessage = EditorGUILayout.Toggle("Log ", logMessage);
        GUILayout.Label("Autofocus makes sure the save script always runs");
        autoFocus = EditorGUILayout.Toggle("Autofocus Window", autoFocus);
    }
}