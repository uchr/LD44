using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosSetting : MonoBehaviour {
    public Color color = Color.yellow;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
    }
}
