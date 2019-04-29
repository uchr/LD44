using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildMonologs : MonoBehaviour {
    public float requiredTime = 0.5f;

    public float timeFromLastSpeach = 0.0f;

    private void Update() {
        bool isActiveQuest = QuestManager.instance.currentQuest != QuestType.None;
        bool playerInTheWild = PlayerState.instance.inTheWild;
        bool isMonolog = MonologManager.instance.isActive;
        if (isActiveQuest && playerInTheWild && !isMonolog) {
            timeFromLastSpeach += Time.deltaTime;
            if (timeFromLastSpeach >= requiredTime) {
                QuestManager.instance.NextMonolog();
            }
        }
        else {
            timeFromLastSpeach = 0.0f;
        }
    }
}
