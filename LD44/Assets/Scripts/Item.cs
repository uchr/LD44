using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour {
    private void Enter() {
        var itemName = gameObject.name;
        if (!BunkerState.items.ContainsKey(itemName)) {
            BunkerState.items.Add(itemName, 0);
        }
        BunkerState.items[itemName]++;
        Destroy(gameObject);
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
        //GetComponentInChildren<TextMeshPro>().text = gameObject.name;
    }
}
