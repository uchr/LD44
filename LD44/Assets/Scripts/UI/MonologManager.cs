using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonologManager : MonoSingleton<MonologManager> {
    public GameObject panel;
    public TextMeshProUGUI message;

    private float timer;

    public void SetText(string text, float time) {
        timer = time;
        panel.SetActive(true);
        message.text = text;
    }

    public void HideText() {
        panel.SetActive(false);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            timer = 0.000001f;

        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) {
                HideText();
            }
        }
    }
}
