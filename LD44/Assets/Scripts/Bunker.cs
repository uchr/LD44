using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class BunkerState {
    public static Dictionary<string, int> items = new Dictionary<string, int>();
}

public class Bunker : MonoBehaviour {
    public GameObject bed;
    public GameObject toilet;
    public GameObject books;

    public TextMeshPro listOfItems;

    private void OnEnable() {
        bed.SetActive(BunkerState.items.ContainsKey("Bed"));
        toilet.SetActive(BunkerState.items.ContainsKey("Toilet"));
        books.SetActive(BunkerState.items.ContainsKey("Books"));

        listOfItems.text = "";
        foreach (var item in BunkerState.items) {
            listOfItems.text += "<b>" + item.Key + "</b> " + item.Value.ToString() + "\n";
        }
    }
}
