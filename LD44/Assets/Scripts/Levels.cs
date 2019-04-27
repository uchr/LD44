using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public GameObject treePrefab;
    
    private void Awake() {
        for (int i = 0; i < 1000; ++i) {
            var position = new Vector3(Random.Range(-100f * 2, 100f * 2), 0, Random.Range(-56.25f * 2, 56.25f * 2));
            GameObject.Instantiate(treePrefab, position, Quaternion.identity); 
        }
    }

    private void Update()
    {
        
    }
}
