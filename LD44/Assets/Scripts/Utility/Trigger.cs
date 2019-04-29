using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
    public UnityEvent enter;
    public UnityEvent exit;

    private void OnTriggerEnter() {
        enter.Invoke();
    }

    private void OnTriggerExit() {
        InteractSystem.instance.HideText();
        exit.Invoke();
    }
}
