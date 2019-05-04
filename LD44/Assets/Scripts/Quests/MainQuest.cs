using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {
    [Header("State")]
    public bool isEnd = false;

    [Header("Final objects")]
    public GameObject rocket;
    public GameObject[] npc;

    public void Start() {
        MonologManager.instance.SetText("Wake up! It's begun! We were preparing for this but everything went wrong. I'm waiting for you in my bunker.", "Begin");
        Stem.MusicManager.Play("Music");
    }

    private void Update() {
        if (WaitItemQuest.instance.isEnd &&
            PlaceItemQuest.instance.isEnd &&
            CollectItemsQuest.instance.isEnd && !isEnd) {
            isEnd = true;

            rocket.SetActive(false);
            foreach (var go in npc)
                go.SetActive(false);

            var end = Stem.SoundManager.GrabSound("Rocket");
            end.Play();
        }
    }
}
