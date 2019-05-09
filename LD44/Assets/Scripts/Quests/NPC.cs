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

        CharacterType character = CharacterType.Player;
        switch (questType) {
            case QuestType.Wait:
                character = CharacterType.Ditto;
                break;
            case QuestType.Place:
                character = CharacterType.UncleVo;
                break;
            case QuestType.Collect:
                character = CharacterType.Vita;
                break;
        }

        if (QuestManager.instance.currentQuest == QuestType.None) {
            if (QuestType.Wait == questType) {
                if (PlaceItemQuest.instance.isEnd && CollectItemsQuest.instance.isEnd)
                    QuestManager.instance.StartQuest(questType);
                else
                    MonologManager.instance.PlayReplica(character, "UncompleteQuest");
            }
            else {
                if (!QuestManager.instance.CheckEnd(questType))
                    QuestManager.instance.StartQuest(questType);
                else
                    MonologManager.instance.PlayReplica(character, "AlreadyEndQuest");
            }
        }
        else {
            if (QuestManager.instance.currentQuest == questType) {
                if (QuestManager.instance.CheckComplete(questType))
                    QuestManager.instance.EndQuest(questType);
                else
                    MonologManager.instance.PlayReplica(character, "QuestInProgress");
            }
            else {
                if (QuestType.Wait == questType)
                    MonologManager.instance.PlayReplica(character, "UncompleteQuest");
                else
                    MonologManager.instance.PlayReplica(character, "HaveAnotherQuest");
            }
        }
    }
}
