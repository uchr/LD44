using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    public QuestType questType;
    public bool isActive = false;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Press <b>E</b> to speak...");
    }

    private void Exit() {
        isActive = false;
    }
    
    private void Awake() {
        GetComponentInChildren<Trigger>().enter.AddListener(Enter);
        GetComponentInChildren<Trigger>().exit.AddListener(Exit);
    }

    public void Update() {
        if (isActive && Input.GetKeyDown(KeyCode.E)) {
            Action();
        }
    }

    private void Action() {
        isActive = false;
        InteractSystem.instance.HideText();

        if (QuestManager.instance.currentQuest == QuestType.None) {
            if (!QuestManager.instance.CheckEnd(questType))
                QuestManager.instance.StartQuest(questType);
            else
                MonologManager.instance.SetText("You're doing fine. Lets help other", 2.5f);
        }
        else {
            if (QuestManager.instance.currentQuest == questType) {
                if (QuestManager.instance.CheckComplete(questType))
                    QuestManager.instance.EndQuest(questType);
                else
                    MonologManager.instance.SetText("Lets do it", 2.5f);
            }
            else {
                MonologManager.instance.SetText("Complete another quest and get back", 2.5f);
            }
        }
    }
}
