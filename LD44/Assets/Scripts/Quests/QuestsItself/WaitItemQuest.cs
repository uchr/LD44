using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitItemQuest : MonoSingleton<WaitItemQuest> {
    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Settings")]
    public CharacterType character;
    public int count = 2;
    public float timeToWait = 3.0f;
    public GameObject[] placesToWait;

    [Header("Dialogs")]
    public string startMessage;
    public string[] wildMonologs;
    public string[] completePart;
    public string endMessage;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    private void Awake() {
        foreach (var go in placesToWait)
            go.SetActive(false);
    }

    public void Scan() {
        count--;
        if (count <= 0)
            isComplete = true;

        if (!isComplete) {
            MonologManager.instance.PlayReplica(character, "Part" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == wildMonologs.Length ? 0 : completePartInd;
        }
        else {
            QuestManager.instance.EndQuest(QuestType.Wait);
        }
    }

    public void StartQuest() {
        MonologManager.instance.PlayReplica(character, "StartQuest");

        foreach (var go in placesToWait)
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
