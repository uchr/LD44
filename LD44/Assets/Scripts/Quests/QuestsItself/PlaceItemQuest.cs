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
    public float timeToPlace = 3.0f;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string[] completePart;
    public string endMessage;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    public void PlaceItem() {
        count--;
        Inventory.instance.RemoveItem(itemForPlacement.name);
        if (count <= 0)
            isComplete = true;

        if (!isComplete) {
            MonologManager.instance.SetText(completePart[completePartInd], "UncleVoPart" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == wildMonologs.Length ? 0 : completePartInd;
        }
        else {
            QuestManager.instance.EndQuest(QuestType.Place);
        }
    }

    public void StartQuest() {
        MonologManager.instance.SetText(startMessage, "UncleVoStartQuest");
        for (int i = 0; i < count; ++i) {
            Inventory.instance.AddItem(itemForPlacement.name);
        }
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.SetText(endMessage, "UncleVoEndQuest");
    }

    public void NextMonolog() {
        MonologManager.instance.SetText(wildMonologs[nextMonologInd], "UncleVoMonolog" + nextMonologInd.ToString());
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologs.Length ? 0 : nextMonologInd;
    }
}
