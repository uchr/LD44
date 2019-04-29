using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal portal;
    public Transform spawnPoint;

    private void Enter() {
        GetComponentInParent<Level>().gameObject.SetActive(false);
        var to = portal.GetComponentsInParent<Level>(true);
        to[0].gameObject.SetActive(true);
        
        to[0].player.transform.position = portal.spawnPoint.position;
    }

    private void Awake() {
        GetComponent<Trigger>().enter.AddListener(Enter);
    }
    
}
