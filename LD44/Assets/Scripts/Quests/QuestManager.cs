using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType {
    Wait,
    Place,
    Collect,
    None
}

public enum QuestStage {
    Start,
    FirstExitFromBunker,
}

public class QuestManager : MonoSingleton<QuestManager> {
    public QuestType currentQuest = QuestType.None;

    public bool StartQuest(QuestType newQuest) {
        if (currentQuest == QuestType.None) {
            currentQuest = newQuest;
            switch (currentQuest) {
                case QuestType.Wait:
                    WaitItemQuest.instance.StartQuest();
                break;
                case QuestType.Place:
                    PlaceItemQuest.instance.StartQuest();
                break;
                case QuestType.Collect:
                    CollectItemsQuest.instance.StartQuest();
                break;
            }
            return true;
        }
        return false;
    }

    public void NextMonolog() {
        switch (currentQuest) {
            case QuestType.Wait:
                //WaitItemQuest.instance.NextMonolog();
            break;
            case QuestType.Place:
                //PlaceItemQuest.instance.NextMonolog();
            break;
            case QuestType.Collect:
                CollectItemsQuest.instance.NextMonolog();
            break;
        }
    }
}
