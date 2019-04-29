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
            switch (newQuest) {
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

    public void EndQuest(QuestType type) {
        if (CheckComplete(type)) {
            switch (type) {
                case QuestType.Wait:
                    //WaitItemQuest.instance.StartQuest();
                break;
                case QuestType.Place:
                    PlaceItemQuest.instance.EndQuest();
                break;
                case QuestType.Collect:
                    CollectItemsQuest.instance.EndQuest();
                break;
            }
            currentQuest = QuestType.None;
        }
    }

    public void NextMonolog() {
        switch (currentQuest) {
            case QuestType.Wait:
                //WaitItemQuest.instance.NextMonolog();
            break;
            case QuestType.Place:
                PlaceItemQuest.instance.NextMonolog();
            break;
            case QuestType.Collect:
                CollectItemsQuest.instance.NextMonolog();
            break;
        }
    }

    public bool CheckComplete(QuestType type) {
        switch (type) {
            case QuestType.Wait:
                //WaitItemQuest.instance.NextMonolog();
                return false;
            case QuestType.Place:
                return PlaceItemQuest.instance.isComplete;
            case QuestType.Collect:
                return CollectItemsQuest.instance.isComplete;
        }
        return false;
    }

    public bool CheckEnd(QuestType type) {
        switch (type) {
            case QuestType.Wait:
                //WaitItemQuest.instance.NextMonolog();
                return false;
            case QuestType.Place:
                return PlaceItemQuest.instance.isEnd;
            case QuestType.Collect:
                return CollectItemsQuest.instance.isEnd;
        }
        return false;
    }
}
