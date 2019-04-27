using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public Transform spawnPoint;
    public GameObject player;

    private void OnEnable() {
        player.transform.position = spawnPoint.position;

    }
}
