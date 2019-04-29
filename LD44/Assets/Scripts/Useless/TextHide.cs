using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHide : MonoBehaviour {
    public bool isInitText = true;

    private void Update() {
        if (isInitText && Input.anyKeyDown) {
            isInitText = false;
            gameObject.SetActive(false);
        }
    }
}
