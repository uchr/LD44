﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public LevelType toLevel;
    private bool isActive = false;

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
        if (isActive && Input.GetKeyDown(KeyCode.E))
            Levels.instance.goTo(toLevel);
    }
}
