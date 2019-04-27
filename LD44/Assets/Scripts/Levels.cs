using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject treePrefab;
    
    private void Awake() {
        for (int i = -2; i <= 2; ++i) {
            for (int j = -2; j <= 2; ++j) {
                var position = new Vector3(100f * i, 0, 56.25f * j);
                var tile = GameObject.Instantiate(tilePrefab, position, Quaternion.identity); 
                var color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
                tile.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }

        for (int i = 0; i < 1000; ++i) {
            var position = new Vector3(Random.Range(-100f * 2, 100f * 2), 0, Random.Range(-56.25f * 2, 56.25f * 2));
            GameObject.Instantiate(treePrefab, position, Quaternion.identity); 
        }
    }

    private void Update()
    {
        
    }
}
