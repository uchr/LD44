using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTrigger : MonoBehaviour
{
    public bool isActive = false;

    private float waitTime = 0.0f;

    private void Enter() {
        isActive = true;
    }

    private void Exit() {
        isActive = false;
    }
    
    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
        GetComponent<Trigger>().exit.AddListener(Exit);
    }

    private void Update() {
        if (isActive && Input.GetKey(KeyCode.E)) {
            waitTime += Time.deltaTime;
            if (waitTime >= WaitItemQuest.instance.timeToWait) {
                WaitItemQuest.instance.PlaceItem();
                GameObject.Instantiate(WaitItemQuest.instance.itemForPlacement, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else {
            waitTime = 0.0f;
        }
    }
}
