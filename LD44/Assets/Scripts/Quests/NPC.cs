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
            if (QuestType.Wait == questType) {
                if (PlaceItemQuest.instance.isEnd && CollectItemsQuest.instance.isEnd) {
                    QuestManager.instance.StartQuest(questType);
                }
                else {
                    MonologManager.instance.SetText("Uncle Wo and my sister needs you now, check me out when you finish his task.", "DittoUncompleteQuest");
                }
            }
            else {
                if (!QuestManager.instance.CheckEnd(questType))
                    QuestManager.instance.StartQuest(questType);
                else {
                    switch (questType) {
                        case QuestType.Place:
                            MonologManager.instance.SetText("It is done! Run to the twins, some measurements and a couple of details will bring our rocket to life.", "UncleVoAlreadyEndQuest");
                        break;
                        case QuestType.Collect:
                            MonologManager.instance.SetText("I almost printed out all 3 seats, the engine and the hull for the rocket. It'll be ready soon. Help Ditto.", "VitaAlreadyEndQuest");
                        break;
                    }
                }
            }
        }
        else {
            if (QuestManager.instance.currentQuest == questType) {
                if (QuestManager.instance.CheckComplete(questType))
                    QuestManager.instance.EndQuest(questType);
                else {
                    switch (questType) {
                        case QuestType.Wait:
                            MonologManager.instance.SetText("I believe you can do it. Just go and do it.", "DittoQuestInProgress");
                            break;
                        case QuestType.Place:
                            MonologManager.instance.SetText("I believe you can do it. Just go and do it.", "UncleVoQuestInProgress");
                            break;
                        case QuestType.Collect:
                            MonologManager.instance.SetText("I believe you can do it. Just go and do it.", "VitaQuestInProgress");
                            break;
                    }
                }
            }
            else {
                switch (questType) {
                    case QuestType.Wait:
                        MonologManager.instance.SetText("Uncle Wo and my sister needs you now, check me out when you finish his task.", "DittoUncompleteQuest");
                        break;
                    case QuestType.Place:
                        MonologManager.instance.SetText("Come back when other tasks are done.", "UncleVoHaveAnotherQuest");
                        break;
                    case QuestType.Collect:
                        MonologManager.instance.SetText("Come back when other tasks are done.", "VitaHaveAnotherQuest");
                        break;
                }
            }
        }
    }
}
