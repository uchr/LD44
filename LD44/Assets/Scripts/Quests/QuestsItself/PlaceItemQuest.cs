using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemQuest : MonoSingleton<PlaceItemQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public GameObject itemForPlacement;
    public int count;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string[] completePart;
    public string completeMessage;
    public string endMessage;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    public void PlaceItem() {
        count--;
        Inventory.instance.RemoveItem(itemForPlacement.name);
        if (count <= 0)
            isComplete = true;

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
        MonologManager.instance.SetText(startMessage, 1.5f);
        for (int i = 0; i < count; ++i) {
            Inventory.instance.AddItem(itemForPlacement.name);
        }
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.SetText(endMessage, 2.0f);
    }

    public void NextMonolog() {
        MonologManager.instance.SetText(wildMonologs[nextMonologInd], 2.0f);
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologs.Length ? 0 : nextMonologInd;
    }
}
