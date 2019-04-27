using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
    public UnityEvent enter;
    public UnityEvent exit;

    private void OnTriggerEnter() {
        enter.Invoke();
        Debug.Log("Enter: " + gameObject.name);
    }

    private void OnTriggerExit() {
        exit.Invoke();
        Debug.Log("Exit: " + gameObject.name);
    }
}
