using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bunker : MonoBehaviour {
    public bool isHome = false;

    private void OnEnable() {
        PlayerState.instance.inHome = true;
    }

    private void OnDisable() {
        if (PlayerState.instance != null)
            PlayerState.instance.inHome = false;
    }
}
