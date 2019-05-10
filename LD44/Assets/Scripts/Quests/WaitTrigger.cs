using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTrigger : MonoBehaviour {
    public bool isActive = false;

    private float waitTime = 0.0f;

    private Stem.SoundInstance waitSound = null;

    private void Enter() {
        isActive = true;
        InteractSystem.instance.SetText("Hold <b>E</b> to scan...");
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
                waitSound = Stem.SoundManager.GrabSound("Scan");
                waitSound.Play();
            }

            waitTime += Time.deltaTime;
            if (waitTime >= WaitItemQuest.instance.timeToWait) {
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
            float procent = Mathf.Floor(100 * waitTime / WaitItemQuest.instance.timeToWait);
            InteractSystem.instance.SetText($"Hold <b>E</b> to scan <b>({procent})</b>...");
        }
    }

    private void Action() {
        waitSound.Stop();
        waitSound = null;

        isActive = false;
        InteractSystem.instance.HideText();

        WaitItemQuest.instance.Scan();
        Destroy(gameObject);
    }
}
