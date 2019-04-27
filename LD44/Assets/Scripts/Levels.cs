using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType {
    Bunker,
    Wild
}

public class Levels : MonoSingleton<Levels> {
    [Header("Levels")]
    public Level bunker;
    public Level wild;

    public void goTo(LevelType type) {
        bool toBunker = type == LevelType.Bunker;
        bunker.gameObject.SetActive(toBunker);
        wild.gameObject.SetActive(!toBunker);

        if (type == LevelType.Bunker)
            Player.instance.transform.position = bunker.spawnPoint.position;
        else 
            Player.instance.transform.position = wild.spawnPoint.position;
    }
}
