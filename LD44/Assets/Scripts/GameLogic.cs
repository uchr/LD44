using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState {
    public static int stage = 0;
}

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject end;
}
