using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemQuest : MonoSingleton<PlaceItemQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public CharacterType character;
    public string radarName = "Disk";
    public int count;
    public float timeToPlace = 3.0f;
    public Radar[] radars;

    [Header("Dialogs")]
    public int wildMonologsCount;
    public int completePartCount;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    public void PlaceItem() {
        count--;
        Inventory.instance.RemoveItem(radarName);
        if (count <= 0)
            isComplete = true;

        if (!isComplete) {
            MonologManager.instance.PlayReplica(character, "Part" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == completePartCount ? 0 : completePartInd;
        }
        else {
            QuestManager.instance.EndQuest(QuestType.Place);
        }
    }

    public void StartQuest() {
        MonologManager.instance.PlayReplica(character, "StartQuest");
        for (int i = 0; i < count; ++i) {
            Inventory.instance.AddItem(radarName);
        }

        foreach (var radar in radars) 
            radar.PlaceStart();
    }

    public void EndQuest() {
        isEnd = true;
        MonologManager.instance.PlayReplica(character, "EndQuest");
    }

    public void NextMonolog() {
        MonologManager.instance.PlayReplica(character, "Monolog" + nextMonologInd.ToString());
        nextMonologInd++;
        nextMonologInd = nextMonologInd == wildMonologsCount ? 0 : nextMonologInd;
    }
}
