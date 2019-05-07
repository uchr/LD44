using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class MonologManager : MonoSingleton<MonologManager> {
    [Header("State")]
    public bool isActive = false;

    [Header("UI")]
    public AudioMixer mixer;
    public GameObject panel;
    public TextMeshProUGUI message;

    [Header("Settings")]
    public float lengthFactor = 1.2f;
    public bool wildState;

    [Header("Timers")]
    public float noiseTimer;
    public float timer;
    public float textTimer;

    private Stem.SoundInstance curBunkerVoice = null;
    private Stem.SoundInstance curRadioVoice = null;

    private Stem.SoundInstance quietNoise = null;

    public void SetText(string text, string voiceName) {
        if (curBunkerVoice != null) {
            curBunkerVoice.Stop();
            curBunkerVoice = null;
        }

        if (curRadioVoice != null) {
            curRadioVoice.Stop();
            curRadioVoice = null;
        }

        isActive = true;

        curBunkerVoice = Stem.SoundManager.GrabSound("Bunker" + voiceName);
        curRadioVoice = Stem.SoundManager.GrabSound("Radio" + voiceName);

        bool playerInTheWild = PlayerState.instance.inTheWild;
        wildState = playerInTheWild;
        noiseTimer = playerInTheWild ? 0.5f : -0.5f;
        if (playerInTheWild) {
            Stem.SoundManager.Play("LoudNoise");
            mixer.FindSnapshot("Radio").TransitionTo(0.0f);
        }
        else {
            mixer.FindSnapshot("Bunker").TransitionTo(0.0f);
            curBunkerVoice.Play();
            curRadioVoice.Play();
        }

        timer = curBunkerVoice.Sound.Variations[0].Clip.length;
        textTimer = curBunkerVoice.Sound.Variations[0].Clip.length * lengthFactor;
        panel.SetActive(true);
        message.text = text;
    }

    private void Start() {
        quietNoise = Stem.SoundManager.GrabSound("QuietNoise");
    }

    private void Update() {
        bool playerInTheWild = PlayerState.instance.inTheWild;
        if (isActive) {
            if (wildState != playerInTheWild) {
                wildState = playerInTheWild;
                if (playerInTheWild) {
                    mixer.FindSnapshot("Radio").TransitionTo(0.2f);
                    quietNoise.Play();
                }
                else {
                    mixer.FindSnapshot("Bunker").TransitionTo(0.2f);
                    quietNoise.Stop();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HideText();
            Stop();
        }

        if (noiseTimer > 0.0f) {
            noiseTimer -= Time.deltaTime;
            if (noiseTimer <= 0.0f) {
                if (curBunkerVoice != null) {
                    curBunkerVoice.Play();
                    curRadioVoice.Play();
                    quietNoise.Play();
                }
            }
        }

        if (noiseTimer <= 0.0f && timer > 0.0f) {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) {
                Stop();

                if (playerInTheWild)
                    Stem.SoundManager.Play("LoudNoise");
                noiseTimer = playerInTheWild ? 0.5f : -0.5f;
            }
        }

        if (textTimer > 0.0f) {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0.0f) {
                HideText();
            }
        }
    }

    private void HideText() {
        textTimer = -1f;

        isActive = false;
        panel.SetActive(false);
    }

    private void Stop() {
        noiseTimer = -1f;
        timer = -1f;

        quietNoise.Stop();
        if (curBunkerVoice != null) {
            curBunkerVoice.Stop();
            curBunkerVoice = null;
        }
        if (curRadioVoice != null) {
            curRadioVoice.Stop();
            curRadioVoice = null;
        }
    }
}
