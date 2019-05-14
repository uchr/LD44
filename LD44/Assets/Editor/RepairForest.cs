using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RepairForest : EditorWindow {
    [MenuItem("SmallRocket/Fix forest")]
    static void FixPrefabs() {
        RandomWild randomWild = GameObject.Find("GeneratedForest").GetComponent<RandomWild>();
        var props = randomWild.props;
        GameObject forest = GameObject.Find("Forest");
        GameObject newForest = new GameObject("NewForest");
        foreach (var t in forest.GetComponentsInChildren<Transform>()) {
            if (t.gameObject.name.Contains("(Clone)")) {
                var index = t.gameObject.name.IndexOf("(Clone)");
                var name = t.gameObject.name.Substring(0, index);
                Debug.Log(name);
                GameObject prefab = null;
                foreach (var go in props) {
                    if (go.name == name) {
                        prefab = go;
                        break;
                    }
                }
                if (prefab != null) {
                    GameObject clone  = PrefabUtility.InstantiatePrefab(prefab as GameObject) as GameObject;
                    clone.transform.SetParent(newForest.transform);
                    clone.transform.position = t.position;
                    clone.transform.rotation = t.rotation;
                    clone.transform.localScale = t.localScale;
                    clone.gameObject.name = name;
                }
                Debug.Log(prefab != null);
            }
        }
        Debug.Log(forest.transform.childCount);
    }
    [MenuItem("SmallRocket/Check forest")]
    static void ChecPrefabs() {
        GameObject forest = GameObject.Find("Forest");
        GameObject newForest = GameObject.Find("NewForest");
        Debug.Log(newForest.transform.childCount);
        Debug.Log(forest.transform.childCount);
    }
}
