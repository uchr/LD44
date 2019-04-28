using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameState {
    public static int stage = 0;
}

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject winGame;
    public GameObject endGame;
    public GameObject homeBunker;

    public void Update() {
        if (PlayerState.instance.curHP <= 0.0f) {
            endGame.SetActive(true);
            Time.timeScale = 0.0f;
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }

        if (BunkerState.items.ContainsKey("Detail0") && 
            BunkerState.items.ContainsKey("Detail1") && 
            BunkerState.items.ContainsKey("Detail2") && 
            homeBunker.activeSelf) {
            
            winGame.SetActive(true);
            Time.timeScale = 0.0f;
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }
}
