﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {
    [Header("State")]
    public bool isEnd = false;

    [Header("Final objects")]
    public GameObject rocket;
    public GameObject[] npc;

    public void Start() {
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
            foreach (var go in npc)
                go.SetActive(false);

            var end = Stem.SoundManager.GrabSound("Rocket");
            end.Play();
        }
    }
}
