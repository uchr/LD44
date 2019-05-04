using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class PlayerState : MonoSingleton<PlayerState> {
    [Header("HP")]
    public float curHP = 100.0f;
    public float maxHP = 100.0f;
    public float hpIncreaseSpeed = 0.0f;
    public float hpDecreaseSpeed = 1.0f;

    [Header("State")]
    public bool inHome;
    public bool inTheWild;

    [Header("UI")]
    public UI.Slider hpBar;

    private void Update() {
        if (inTheWild)
            curHP -= hpDecreaseSpeed * Time.deltaTime;
        else
            curHP += hpIncreaseSpeed * Time.deltaTime;

        hpBar.value = curHP;
        hpBar.maxValue = maxHP;
    }
}
