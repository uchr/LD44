using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosSetting : MonoBehaviour {
    public Color color = Color.yellow;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        float avarage = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 6;
        Gizmos.DrawSphere(transform.position, avarage);
    }
}
