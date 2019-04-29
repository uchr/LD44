using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitItemQuest : MonoSingleton<WaitItemQuest> {
    public bool isSolve;

    public GameObject itemForPlacement;
    public int count = 2;
    public float timeToWait = 3.0f;

    private void Awake() {
        for (int i = 0; i < count; ++i) {
            Inventory.instance.AddItem(itemForPlacement.name);
        }
    }

    public void PlaceItem() {
        count--;
        Inventory.instance.RemoveItem(itemForPlacement.name);
        if (count <= 0)
            isSolve = true;
    }

    public void StartQuest() {
        MonologManager.instance.SetText("Scan pls", 1.5f);
    }
}
