using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildMonologs : MonoBehaviour {
    public float pauseTime = 5.0f;

    private float timeFromLastSpeach = 0.0f;

    private void Update() {
        bool isActiveQuest = QuestManager.instance.currentQuest != QuestType.None;
        bool playerInTheWild = PlayerState.instance.inTheWild;
        bool isMonolog = MonologManager.instance.isActive;
        if (isActiveQuest && playerInTheWild && !isMonolog) {
            timeFromLastSpeach += Time.deltaTime;
            if (timeFromLastSpeach >= pauseTime) {
                QuestManager.instance.NextMonolog();
            }
        }
        else {
            timeFromLastSpeach = 0.0f;
        }
    }
}
