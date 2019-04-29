using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWild : MonoBehaviour {
    public int numberOfProps = 100;
    public float size = -50f;
    public GameObject[] props;

    private void Awake() {
        for (int i = 0; i < numberOfProps; ++i) {
            var pos = new Vector3(Random.Range(-size, size), 0.0f, Random.Range(-size, size));
            GameObject prop = props[Random.Range(0, props.Length)];
            GameObject.Instantiate(prop, pos, Quaternion.identity);
        }
    }
}
