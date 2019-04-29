using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoSingleton<GameLogic> {
    public GameObject winGame;
    public GameObject endGame;
    public GameObject homeBunker;

    private void Awake() {
        Time.timeScale = 1.0f;
    }

    private void Update() {
        if (PlayerState.instance.curHP <= 0.0f) {
            endGame.SetActive(true);
            Time.timeScale = 0.0f;
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }

        if (WaitItemQuest.instance.isEnd && 
            PlaceItemQuest.instance.isEnd &&
            CollectItemsQuest.instance.isEnd) {
            winGame.SetActive(true);
            Time.timeScale = 0.0f;
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        }
    }
}
