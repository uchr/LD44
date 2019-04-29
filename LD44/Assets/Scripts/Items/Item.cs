using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour {
    private void Enter() {
        Inventory.instance.AddItem(gameObject.name);
        Destroy(gameObject);
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
    }
}
