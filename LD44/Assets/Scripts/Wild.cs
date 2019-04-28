using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : MonoBehaviour {
    public GameObject[] stages;
    
    public List<List<GameObject>> items = new List<List<GameObject>>();

    private void Awake() {
        for (int i = 0; i < stages.Length; ++i) {
            List<GameObject> stageItems = new List<GameObject>();
            foreach (var item in stages[i].GetComponentsInChildren<Item>()) {
                stageItems.Add(item.gameObject);
                item.gameObject.SetActive(false);
            }
            items.Add(stageItems);
        }
    }

    private void OnEnable() {
        int stage = GameLogic.instance.stage;
        foreach (var go in items[stage])
            go.SetActive(true);
    }

    private void OnDisable() {
        int stage = GameLogic.instance.stage;
        foreach (var go in items[stage])
            if (go != null)
                go.SetActive(false);

        GameLogic.instance.stage++;
    }
}
