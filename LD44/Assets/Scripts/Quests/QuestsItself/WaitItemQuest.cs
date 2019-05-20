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
    public Radar[] radars;

    [Header("Dialogs")]
    public int wildMonologsCount;
    public int completePartCount;

    private int nextMonologInd = 0;
    private int completePartInd = 0;

    public void Scan() {
        count--;
        if (count <= 0)
            isComplete = true;

        if (!isComplete) {
            MonologManager.instance.PlayReplica(character, "Part" + completePartInd.ToString());
            completePartInd++;
            completePartInd = completePartInd == completePartCount ? 0 : completePartInd;
        }
        else {
            QuestManager.instance.EndQuest(QuestType.Wait);
        }
    }

    public void StartQuest() {
        MonologManager.instance.PlayReplica(character, "StartQuest");

        foreach (var radar in radars)
            radar.WaitStart();
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
