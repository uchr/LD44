using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour {
    private void Enter() {
        var itemName = gameObject.name;
        if (!PlayerState.instance.items.ContainsKey(itemName)) {
            PlayerState.instance.items.Add(itemName, 0);
        }
        PlayerState.instance.items[itemName]++;
        Destroy(gameObject);
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
        //GetComponentInChildren<TextMeshPro>().text = gameObject.name;
    }
}
