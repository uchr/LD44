using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameState {
    public static int stage = 0;
}

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject endGame;

    public void Update() {
        if (PlayerState.instance.curHP <= 0.0f) {
            endGame.SetActive(true);
            Time.timeScale = 0.0f;
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }
}
