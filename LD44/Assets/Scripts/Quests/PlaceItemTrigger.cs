using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemTrigger : MonoBehaviour
{
    public bool isActive = false;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Press <b>E</b> to place...");
    }

    private void Exit() {
        isActive = false;
    }
    
    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
        GetComponent<Trigger>().exit.AddListener(Exit);
    }

    public void Update() {
        if (isActive && Input.GetKeyDown(KeyCode.E)) {
            Action();
        }
    }

    private void Action() {
        isActive = false;
        InteractSystem.instance.HideText();

        PlaceItemQuest.instance.PlaceItem();
        GameObject.Instantiate(PlaceItemQuest.instance.itemForPlacement, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
