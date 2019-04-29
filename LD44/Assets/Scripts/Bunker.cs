using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class BunkerState {
    public static Dictionary<string, int> items = new Dictionary<string, int>();
}

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
