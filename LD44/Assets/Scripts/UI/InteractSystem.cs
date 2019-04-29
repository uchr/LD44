using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractSystem : MonoSingleton<InteractSystem> {
    public GameObject panel;
    public TextMeshProUGUI message;

    public void SetText(string text) {
        panel.SetActive(true);
        message.text = text;
    }

    public void HideText() {
        panel.SetActive(false);
    }
}
