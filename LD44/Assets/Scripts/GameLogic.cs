using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerState {
    public static int hp;
    public static int oxygen;
}

public class GameLogic : MonoSingleton<GameLogic> {
    public int stage = 0;

    public GameObject end;
}
