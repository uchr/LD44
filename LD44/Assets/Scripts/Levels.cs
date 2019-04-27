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
        if (type == LevelType.Wild && GameLogic.instance.stage == 3) {
            GameLogic.instance.end.SetActive(true);
        }
        else {
            bool toBunker = type == LevelType.Bunker;
            bunker.gameObject.SetActive(toBunker);
            wild.gameObject.SetActive(!toBunker);
        }
    }
}
