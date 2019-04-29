using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemsQuest : MonoSingleton<CollectItemsQuest> {
    [Header("State")]
    public bool isSolve;

    [Header("Settings")]
    public List<string> requiredItems;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string complete;
    public string end;

    public string getDetail0;
    public string getDetail1;
    public string getDetail2;

    private int nextMonolog = 0;
    private Dictionary<string, bool> existItems = new Dictionary<string, bool>();

    private void Awake() {
        Inventory.instance.addItem.AddListener(CheckItems);

        foreach (var itemName in requiredItems) {
            existItems[itemName] = false;
        }
    }

    public void CheckItems(string itemName, int itemCount) {
        existItems[itemName] = true;

        bool check = true;
        foreach (var item in existItems) {
            check &= item.Value;
        }
        isSolve = check;

        if (!isSolve) {
            if (itemName == "Detail0")
                MonologManager.instance.SetText(getDetail0, 2.0f);
            if (itemName == "Detail1")
                MonologManager.instance.SetText(getDetail1, 2.0f);
            if (itemName == "Detail2")
                MonologManager.instance.SetText(getDetail2, 2.0f);
        }
        else {
            MonologManager.instance.SetText(complete, 2.0f);
        }
    }

    public void StartQuest() {
        MonologManager.instance.SetText(startMessage, 2.0f);
    }

    public void NextMonolog() {
        MonologManager.instance.SetText(wildMonologs[nextMonolog], 2.0f);
        nextMonolog++;
        nextMonolog = nextMonolog == wildMonologs.Length ? 0 : nextMonolog;
    }
}
