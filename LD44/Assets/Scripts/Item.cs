using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public string itemName;
    
    private void Enter() {
        BunkerState.items.Add(itemName);
        Destroy(gameObject);
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
    }
}
