using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonologManager : MonoSingleton<MonologManager> {
    public bool isActive = false;
    public GameObject panel;
    public TextMeshProUGUI message;

    private float timer;
    private Stem.SoundInstance curVoice = null;

    public void SetText(string text, string voiceName) {
        if (curVoice != null) {
            curVoice.Stop();
            curVoice = null;
        }

        isActive = true;

        curVoice = Stem.SoundManager.GrabSound(voiceName);
        curVoice.Play();
        timer = curVoice.Sound.Variations[0].Clip.length;
        panel.SetActive(true);
        message.text = text;
    }

    public void HideText() {
        isActive = false;
        panel.SetActive(false);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            timer = 0.000001f;

        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) {
                if (curVoice != null) {
                    curVoice.Stop();
                    curVoice = null;
                }
                HideText();
            }
        }
    }
}
