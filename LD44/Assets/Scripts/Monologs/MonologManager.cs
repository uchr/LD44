using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class MonologManager : MonoSingleton<MonologManager> {
    [Header("State")]
    public bool isActive = false;

    [Header("Data")]
    public MonologData vitaData;
    public MonologData dittoData;
    public MonologData uncleVoData;

    [Header("UI")]
    public AudioMixer mixer;
    public GameObject panel;
    public TextMeshProUGUI message;

    public GameObject portraitUncleVo;
    public GameObject portraitVita;
    public GameObject portraitDitto;

    [Header("Settings")]
    public float lengthFactor = 1.2f;
    public CharacterType replicaCharachter;
    public bool currentRadioMode = false;

    [Header("Timers")]
    public float noiseTimer;
    public float timer;
    public float textTimer;

    private Stem.SoundInstance curBunkerVoice = null;
    private Stem.SoundInstance curRadioVoice = null;

    private Stem.SoundInstance quietNoise = null;

    public void PlayReplica(CharacterType character, string key) {
        portraitUncleVo.SetActive(false);
        portraitVita.SetActive(false);
        portraitDitto.SetActive(false);

        MonologData data = null;
        switch (character) {
            case CharacterType.Ditto:
                data = dittoData;
                portraitDitto.SetActive(true);
                break;
            case CharacterType.Vita:
                data = vitaData;
                portraitVita.SetActive(true);
                break;
            case CharacterType.UncleVo:
                data = uncleVoData;
                portraitUncleVo.SetActive(true);
                break;
        }

        Replica replica = null;
        foreach (var r in data.replics) 
            if (r.key == key)
                replica = r;
        
        if (replica == null) {
            Debug.LogError("Invalid key!");
            return;
        }

        replicaCharachter = character;
        SetText(replica.text, character.ToString() + replica.key);
    }

    private void SetText(string text, string voiceKey) {
        if (curBunkerVoice != null) {
            curBunkerVoice.Stop();
            curBunkerVoice = null;
        }

        if (curRadioVoice != null) {
            curRadioVoice.Stop();
            curRadioVoice = null;
        }

        isActive = true;

        curBunkerVoice = Stem.SoundManager.GrabSound("Bunker" + voiceKey);
        curRadioVoice = Stem.SoundManager.GrabSound("Radio" + voiceKey);

        bool inTheWild = PlayerState.instance.inTheWild;
        currentRadioMode = inTheWild || (!inTheWild && PlayerState.instance.currentBunker != replicaCharachter);
        noiseTimer = currentRadioMode ? 0.5f : -0.5f;
        if (currentRadioMode) {
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
        bool inTheWild = PlayerState.instance.inTheWild;
        bool radioMode = inTheWild || (!inTheWild && PlayerState.instance.currentBunker != replicaCharachter);
        if (isActive) {
            if (currentRadioMode != radioMode) {
                currentRadioMode = radioMode;
                if (currentRadioMode) {
                    mixer.FindSnapshot("Radio").TransitionTo(0.1f);
                    quietNoise.Play();
                }
                else {
                    mixer.FindSnapshot("Bunker").TransitionTo(0.1f);
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
                    if (currentRadioMode)
                        quietNoise.Play();
                }
            }
        }

        if (noiseTimer <= 0.0f && timer > 0.0f) {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) {
                Stop();

                if (currentRadioMode)
                    Stem.SoundManager.Play("LoudNoise");
                noiseTimer = currentRadioMode ? 0.5f : -0.5f;
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
