using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal portal;
    public Transform spawnPoint;

    private bool isActive = false;

    private void Enter() {
        isActive = true;
    }

    private void Exit() {
        isActive = false;
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
        GetComponent<Trigger>().exit.AddListener(Exit);
    }
    
    private void Update() {
        if (isActive && Input.GetKeyDown(KeyCode.E)) {
            isActive = false;
            
            GetComponentInParent<Level>().gameObject.SetActive(false);
            var to = portal.GetComponentsInParent<Level>(true);
            to[0].gameObject.SetActive(true);
            
            to[0].player.transform.position = portal.spawnPoint.position;
        }
    }
}
