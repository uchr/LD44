using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class PlayerState : MonoSingleton<PlayerState> {
    [Header("Items")]
    public Dictionary<string, int> items = new Dictionary<string, int>();

    [Header("HP")]
    public float curHP = 100.0f;
    public float maxHP = 100.0f;
    public float hpIncreaseSpeed = 0.0f;
    public float hpDecreaseSpeed = 1.0f;

    [Header("Oxygen")]
    public float curOxygen = 100.0f;
    public float maxOxygen = 100.0f;
    public float oxygenIncreaseSpeed = 2.0f;
    public float oxygenDecreaseSpeed = 1.0f;

    [Header("State")]
    public bool inTheWild;
    public bool inHome;

    [Header("UI")]
    public UI.Slider hpBar;
    public UI.Slider oxygenBar;

    [Header("Circle")]
    public Map map;
    public float[] circle;

    private int prevBallon = 0;

    private void Update() {
        int balloon = 0;
        if (items.ContainsKey("OxygenBalloon"))
            balloon = items["OxygenBalloon"];

        if (prevBallon != balloon) {
            map.oxygenSize = circle[balloon];
            maxOxygen = 100.0f + balloon * 50.0f;
            oxygenBar.GetComponent<RectTransform>().sizeDelta = new Vector2 (maxOxygen * 2, oxygenBar.GetComponent<RectTransform>().rect.height);
            prevBallon = balloon;
        }

        if (inTheWild) {
            if (curOxygen > 0.0f) {
                curOxygen -= oxygenDecreaseSpeed * Time.deltaTime;
            }
            else {
                curHP -= hpDecreaseSpeed * Time.deltaTime;
            }
        }

        if (inHome) {
            if (curOxygen < maxOxygen) {
                curOxygen += oxygenIncreaseSpeed * Time.deltaTime;
            }
        }

        hpBar.value = curHP;
        hpBar.maxValue = maxHP;
        
        oxygenBar.value = curOxygen;
        oxygenBar.maxValue = maxOxygen;
    }
}
