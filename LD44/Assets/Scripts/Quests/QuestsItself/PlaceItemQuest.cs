using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemQuest : MonoSingleton<PlaceItemQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public CharacterType character;
    public GameObject itemForPlacement;
    public int count;
    public float timeToPlace = 3.0f;
    public GameObject[] placesToPlace;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string[] completePart;
    public string endMessage;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    private void Awake() {
        foreach (var go in placesToPlace)
            go.SetActive(false);
    }

    public void PlaceItem() {
        count--;
        Inventory.instance.RemoveItem(itemForPlacement.name);
        if (count <= 0)
            isComplete = true;

        if (!isComplete) {
            MonologManager.instance.PlayReplica(character, "Part" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == wildMonologs.Length ? 0 : completePartInd;
        }
        else {
            QuestManager.instance.EndQuest(QuestType.Place);
        }
    }

    public void StartQuest() {
        MonologManager.instance.PlayReplica(character, "StartQuest");
        for (int i = 0; i < count; ++i) {
            Inventory.instance.AddItem(itemForPlacement.name);
        }

        foreach (var go in placesToPlace)
            go.SetActive(true);
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.PlayReplica(character, "EndQuest");
    }

    public void NextMonolog() {
        MonologManager.instance.PlayReplica(character, "Monolog" + nextMonologInd.ToString());
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologs.Length ? 0 : nextMonologInd;
    }
}
