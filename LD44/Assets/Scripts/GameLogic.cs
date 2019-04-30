using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject winGame;
    public GameObject endGame;
    public GameObject rocket;

    private bool isEnd = false;

    private void Start() {
        var begin = Stem.SoundManager.GrabSound("Begin");
        begin.Play();
        Stem.MusicManager.Play("Music");
    }

    private void Update() {
        if (WaitItemQuest.instance.isEnd && 
            PlaceItemQuest.instance.isEnd &&
            CollectItemsQuest.instance.isEnd && !isEnd) {
                isEnd = true;
                rocket.SetActive(false);
                var end = Stem.SoundManager.GrabSound("Rocket");
                end.Play();
        }
    }
}
