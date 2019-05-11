using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoSingleton<MainQuest> {
    [Header("Debug")]
    public bool disableStartSound = false;

    [Header("State")]
    public bool isComplete = false;
    public bool isEnd = false;

    [Header("Final objects")]
    public GameObject burnedPlace;
    public GameObject rocket;
    public GameObject[] npc;
    public GameObject[] endSign;
    
    [Header("UI")]
    public GameObject finalMessageScreen;
    public GameObject thankYouScreen;

    private void Start() {
        if (!disableStartSound) {
            MonologManager.instance.PlayReplica(CharacterType.UncleVo, "StartMainQuest");
            Stem.MusicManager.Play("Music");
        }
    }

    private void Update() {
        if (WaitItemQuest.instance.isEnd &&
            PlaceItemQuest.instance.isEnd &&
            CollectItemsQuest.instance.isEnd && !isComplete) {
            isComplete = true;

            burnedPlace.SetActive(true);
            foreach (var go in endSign)
                go.SetActive(true);

            rocket.SetActive(false);
            foreach (var go in npc)
                go.SetActive(false);

            var end = Stem.SoundManager.GrabSound("Rocket");
            end.Play();
        }
    }

    public void End() {
        isEnd = true;
        finalMessageScreen.SetActive(true);
    }

    public void ThankYou() {
        finalMessageScreen.SetActive(false);
        thankYouScreen.SetActive(true);
    }
}
