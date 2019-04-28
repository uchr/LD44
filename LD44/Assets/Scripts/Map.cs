using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public Transform playerIcon;
    public Level wild;
    public RectTransform oxygenMask;
    public float magicFactor = 0.5f;
    public float oxygenSize = 100.0f;

    private void Update() {
        oxygenMask.sizeDelta = new Vector2(oxygenSize, oxygenSize);
        if (wild.gameObject.activeSelf) {
            float factor = (1 / magicFactor);
            playerIcon.localPosition = factor * new Vector3(wild.player.transform.position.x, wild.player.transform.position.z, 0f);
        }
        else {
            playerIcon.localPosition = Vector3.zero;
        }
    }
}
