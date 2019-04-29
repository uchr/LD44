using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemsQuest : MonoSingleton<CollectItemsQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public List<string> requiredItems;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string[] completePart;
    public string completeMessage;
    public string endMessage;

    private int nextMonologInd = 0;
    private int completePartInd = 0;
    private Dictionary<string, bool> existItems = new Dictionary<string, bool>();

    private void Awake() {
        Inventory.instance.addItem.AddListener(CheckItems);

        foreach (var itemName in requiredItems) {
            existItems[itemName] = false;
        }
    }

    public void CheckItems(string itemName, int itemCount) {
        if (itemName != "Detail0" && itemName != "Detail1" && itemName != "Detail2")
            return;

        existItems[itemName] = true;

        bool check = true;
        foreach (var item in existItems) {
            check &= item.Value;
        }
        isComplete = check;

        if (!isComplete) {
            MonologManager.instance.SetText(completePart[completePartInd], 2.0f);
            completePartInd++;
            completePartInd = completePartInd == wildMonologs.Length ? 0 : completePartInd;
        }
        else {
            MonologManager.instance.SetText(completeMessage, 2.0f);
        }
    }

    public void StartQuest() {
        MonologManager.instance.SetText(startMessage, 2.0f);
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.SetText(endMessage, 2.0f);
        Inventory.instance.RemoveItem("Detail0");
        Inventory.instance.RemoveItem("Detail1");
        Inventory.instance.RemoveItem("Detail2");
    }

    public void NextMonolog() {
        MonologManager.instance.SetText(wildMonologs[nextMonologInd], 2.0f);
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologs.Length ? 0 : nextMonologInd;
    }
}
