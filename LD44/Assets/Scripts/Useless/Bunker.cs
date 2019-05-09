using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Character {
    Ditto,
    Vita,
    UncleVo,
    Player
}

public class Bunker : MonoBehaviour {
    public Character owner;

    private void OnEnable() {
        PlayerState.instance.currentBunker = owner;
    }
}
