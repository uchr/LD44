using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal portal;
    public Transform spawnPoint;

    public bool isActive = false;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Press <b>E</b> to enter...");
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

        GetComponentInParent<Level>().gameObject.SetActive(false);
        var to = portal.GetComponentsInParent<Level>(true);
        to[0].gameObject.SetActive(true);
        to[0].player.transform.position = portal.spawnPoint.position;
        Stem.SoundManager.Play("Door");
    }

}
