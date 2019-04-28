using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class BunkerState {
    public static Dictionary<string, int> items = new Dictionary<string, int>();
}

public class Bunker : MonoBehaviour {
    /*private void OnEnable() {
        listOfItems.text = "";
        foreach (var item in BunkerState.items) {
            listOfItems.text += "<b>" + item.Key + "</b> " + item.Value.ToString() + "\n";
        }
    }*/
}
