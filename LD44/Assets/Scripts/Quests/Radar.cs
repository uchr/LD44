using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {
    public GameObject visual;
    public GameObject placeTrigger;
    public GameObject waitTrigger;
    public HighlightingSystem.Highlighter crossHighlighter;
    public HighlightingSystem.Highlighter radarHighlighter;

    public void PlaceStart() {
        crossHighlighter.enabled = true;
        placeTrigger.SetActive(true);
    }

    public void PlaceEnd() {
        visual.SetActive(true);
        crossHighlighter.enabled = false;
    }

    public void WaitStart() {
        radarHighlighter.enabled = true;
        waitTrigger.SetActive(true);
    }

    public void WaitEnd() {
        radarHighlighter.enabled = false;
    }
}
