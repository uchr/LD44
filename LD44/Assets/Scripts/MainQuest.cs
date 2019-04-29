using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuest : MonoBehaviour {
    public void Start() {
        var begin = Stem.SoundManager.GrabSound("Begin");
        begin.Play();
        Stem.MusicManager.Play("Music");
    }
}
