using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemTrigger : MonoBehaviour {
    public bool isActive = false;

    private float waitTime = 0.0f;
    
    private Stem.SoundInstance waitSound = null;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Hold <b>E</b> to place...");
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
            if (waitSound == null) {
                waitSound = Stem.SoundManager.GrabSound("Place");
                waitSound.Play();
            }

            waitTime += Time.deltaTime;
            if (waitTime >= PlaceItemQuest.instance.timeToPlace) {
                Action();
            }
        }
        else {
            if (waitSound != null) {
                waitSound.Stop();
                waitSound = null;
            }
            waitTime = 0.0f;
        }

        if (isActive) {
            float procent = Mathf.Floor(100 * waitTime / PlaceItemQuest.instance.timeToPlace);
            InteractSystem.instance.SetText($"Hold <b>E</b> to place <b>({procent})</b>...");
        }
    }

    private void Action() {
        waitSound.Stop();
        waitSound = null;

        isActive = false;
        InteractSystem.instance.HideText();

        PlaceItemQuest.instance.PlaceItem();
        GameObject.Instantiate(PlaceItemQuest.instance.itemForPlacement, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
