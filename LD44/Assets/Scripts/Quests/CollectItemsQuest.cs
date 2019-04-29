using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemsQuest : MonoSingleton<CollectItemsQuest>
{
    public bool isSolve;
    public List<string> requiredItems;

    private Dictionary<string, bool> existItems = new Dictionary<string, bool>();

    private void Awake() {
        Inventory.instance.addItem.AddListener(CheckItems);

        foreach (var itemName in requiredItems) {
            existItems[itemName] = false;
        }
    }

    public void CheckItems(string itemName, int itemCount) {
        if (requiredItems.Contains(itemName)) {
            existItems[itemName] = true;
        }

        bool check = true;
        foreach (var item in existItems) {
            check &= item.Value;
        }

        isSolve = check;
    }
}
