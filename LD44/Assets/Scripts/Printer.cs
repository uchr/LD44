using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Printer : MonoBehaviour {
    public string desc;
    public bool isInit = false;

    public GameObject textGO;
    public TextMeshPro text;

    private void Enter() {
        text.text = desc;
        textGO.SetActive(true);
    }

    private void Exit() {
        textGO.SetActive(false);
    }

    private void Awake() {
        GetComponentInChildren<Trigger>().enter.AddListener(Enter);
        GetComponentInChildren<Trigger>().exit.AddListener(Exit);
    }
}
