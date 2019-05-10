#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonologData))]
public class MonologEditor : Editor {
    MonologData monologData;

    public void OnEnable() {
        monologData = (MonologData) target;
        if (monologData.replics == null)
            monologData.replics = new List<Replica>();
    }

    public override void OnInspectorGUI() {
        monologData.character = (CharacterType) EditorGUILayout.EnumPopup("Character ", monologData.character);
        GUILayout.Space(10);

        for (int i = 0; i < monologData.replics.Count; ++i) {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            monologData.replics[i].key = EditorGUILayout.TextField("Key", monologData.replics[i].key);
            monologData.replics[i].text = EditorGUILayout.TextField("Text", monologData.replics[i].text);

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            
            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            if (GUILayout.Button("Up") && i > 0) {
                var t = monologData.replics[i - 1];
                monologData.replics[i - 1] = monologData.replics[i];
                monologData.replics[i] = t;
            }
            if (GUILayout.Button("Down") && i < monologData.replics.Count - 1) {
                var t = monologData.replics[i + 1];
                monologData.replics[i + 1] = monologData.replics[i];
                monologData.replics[i] = t;
            }
            if (GUILayout.Button("Remove")) {
                monologData.replics.RemoveAt(i);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add")) {
            Replica newReplica = new Replica();
            newReplica.key = "NewRelica";
            newReplica.text = "Text";
            monologData.replics.Add(newReplica);
        }

        EditorUtility.SetDirty(monologData);
    }
}
#endif