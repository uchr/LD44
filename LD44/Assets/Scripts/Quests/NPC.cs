using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    public QuestType questType;
    public bool isActive = false;

    private void Enter() {
        isActive = true;
    }

    private void Exit() {
        isActive = false;
    }
    
    private void Awake() {
        GetComponentInChildren<Trigger>().enter.AddListener(Enter);
        GetComponentInChildren<Trigger>().exit.AddListener(Exit);
    }

    public void Update() {
        if (isActive && Input.GetKey(KeyCode.E)) {
            QuestManager.instance.StartQuest(questType);
        }
    }
}
