using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType {
    Wait,
    Place,
    Collect,
    None
}

public class QuestManager : MonoSingleton<QuestManager> {
    public QuestType currentQuest = QuestType.None;

    public bool StartQuest(QuestType newQuest) {
        if (currentQuest == QuestType.None) {
            currentQuest = newQuest;
            return true;
        }
        return false;
    }
}
