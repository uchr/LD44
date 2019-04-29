using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : MonoBehaviour {
    private void OnEnable() {
        PlayerState.instance.inTheWild = true;
    }

    private void OnDisable() {
        if (PlayerState.instance != null)
            PlayerState.instance.inTheWild = false;
    }
}
