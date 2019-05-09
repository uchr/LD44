using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {
    [Header("Debug")]
    public bool disableStartSound = false;

    [Header("State")]
    public bool isEnd = false;

    [Header("Final objects")]
    public GameObject burnedPlace;
    public GameObject rocket;
    public GameObject[] npc;

    public void Start() {
        if (!disableStartSound) {
            MonologManager.instance.PlayReplica(CharacterType.Ditto, "StartMainQuest");
            Stem.MusicManager.Play("Music");
        }
    }

    private void Update() {
        if (WaitItemQuest.instance.isEnd &&
            PlaceItemQuest.instance.isEnd &&
            CollectItemsQuest.instance.isEnd && !isEnd) {
            isEnd = true;

            burnedPlace.SetActive(true);
            rocket.SetActive(false);
            foreach (var go in npc)
                go.SetActive(false);

            var end = Stem.SoundManager.GrabSound("Rocket");
            end.Play();
        }
    }
}
