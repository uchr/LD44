using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BunkerState {
    public static List<string> items = new List<string>();
}

public class Bunker : MonoBehaviour {
    public GameObject bed;
    public GameObject toilet;
    public GameObject books;

    private void OnEnable() {
        bed.SetActive(BunkerState.items.Contains("Bed"));
        toilet.SetActive(BunkerState.items.Contains("Toilet"));
        books.SetActive(BunkerState.items.Contains("Books"));
    }
}
