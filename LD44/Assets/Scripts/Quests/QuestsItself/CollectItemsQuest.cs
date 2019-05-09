using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemsQuest : MonoSingleton<CollectItemsQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public CharacterType character;
    public List<string> requiredItems;
    public GameObject[] detailsObject;

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

        foreach (var go in detailsObject)
            go.SetActive(false);
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
            MonologManager.instance.PlayReplica(character, "Part" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == wildMonologs.Length ? 0 : completePartInd;
        }
        else {
            MonologManager.instance.PlayReplica(character, "CompleteQuest");
        }
    }

    public void StartQuest() {
        MonologManager.instance.PlayReplica(character, "StartQuest");
        foreach (var go in detailsObject)
            go.SetActive(true);
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.PlayReplica(character, "EndQuest");
        Inventory.instance.RemoveItem("Detail0");
        Inventory.instance.RemoveItem("Detail1");
        Inventory.instance.RemoveItem("Detail2");
    }

    public void NextMonolog() {
        MonologManager.instance.PlayReplica(character, "Monolog" + nextMonologInd.ToString());
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologs.Length ? 0 : nextMonologInd;
    }
}
