using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : MonoBehaviour {
    public GameObject[] stage0;
    public GameObject[] stage1;
    public GameObject[] stage2;
    
    public List<GameObject[]> items = new List<GameObject[]>();

    private void Awake() {
        items.Add(stage0);
        items.Add(stage1);
        items.Add(stage2);
    }

    private void OnEnable() {
        int stage = GameLogic.instance.stage;
        foreach (var go in items[stage])
            go.SetActive(true);
    }

    private void OnDisable() {
        GameLogic.instance.stage++;
    }
}
