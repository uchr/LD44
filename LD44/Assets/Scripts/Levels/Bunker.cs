using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CharacterType {
    Ditto,
    Vita,
    UncleVo,
    Player
}

public class Bunker : MonoBehaviour {
    public CharacterType owner;

    private void OnEnable() {
        PlayerState.instance.currentBunker = owner;
    }
}
