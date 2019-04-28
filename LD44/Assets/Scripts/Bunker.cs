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
        if (isHome)
            PlayerState.instance.inHome = true;
        /*listOfItems.text = "";
        foreach (var item in BunkerState.items) {
            listOfItems.text += "<b>" + item.Key + "</b> " + item.Value.ToString() + "\n";
        }*/
    }

    private void OnDisable() {
        PlayerState.instance.inHome = false;
    }
}
