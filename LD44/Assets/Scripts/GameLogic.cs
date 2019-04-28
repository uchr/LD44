using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerState {
    public static int hp = 100;
    public static int oxygen = 100;
}

public static class GameState {
    public static int stage = 0;
}

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject end;
}
