using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemQuest : MonoSingleton<PlaceItemQuest> {
    public bool isSolve;

    public GameObject itemForPlacement;
    public int count;

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
}
