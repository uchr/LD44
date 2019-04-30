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
                    MonologManager.instance.SetText("Uncle Wo needs you now, check me out when you finish his task.", "DittoNotReadu");
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
                            MonologManager.instance.SetText("I almost printed out all the parts of the rocket. Soon she will be ready. Help Ditto.", "VitaAlreadyEndQuest");
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
                            MonologManager.instance.SetText("I believe that you can handle it. Just do it.", "DittoContinueQuest");
                            break;
                        case QuestType.Place:
                            MonologManager.instance.SetText("I believe that you can handle it. Just do it.", "UncleVoContinueQuest");
                            break;
                        case QuestType.Collect:
                            MonologManager.instance.SetText("I believe that you can handle it. Just do it.", "VitaContinute");
                            break;
                    }
                }
            }
            else {
                switch (questType) {
                    case QuestType.Wait:
                        MonologManager.instance.SetText("Uncle Wo needs you now, check me out when you finish his task.", "DittoNotReadu");
                        break;
                    case QuestType.Place:
                        MonologManager.instance.SetText("Come back when other things are done.", "UncleVoAnotherQuest");
                        break;
                    case QuestType.Collect:
                        MonologManager.instance.SetText("Come back when other things are done.", "VitaAlreadyQuest");
                        break;
                }
            }
        }
    }
}
