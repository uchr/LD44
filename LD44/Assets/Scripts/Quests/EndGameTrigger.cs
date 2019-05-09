using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour {
    public bool isActive = false;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Press <b>E</b> to read...");
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

        MainQuest.instance.End();
    }
}
