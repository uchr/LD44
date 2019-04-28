using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir {
    Forward,
    Backward,
    Right,
    Left
}

public class PlayerMovement : MonoBehaviour {
    public float speed;

    public GameObject forward;
    public GameObject side;
    public GameObject backward;

    public bool isWild = false;

    private Rigidbody rb;
    private Animator animatorForward;
    private Animator animatorSide;
    private Animator animatorBackward;

    public Dir dir;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animatorForward = forward.GetComponentInChildren<Animator>();
        animatorSide = side.GetComponentInChildren<Animator>();
        animatorBackward = backward.GetComponentInChildren<Animator>();

        forward.SetActive(false);
        side.SetActive(false);
        backward.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            dir = Dir.Backward;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            dir = Dir.Forward;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            dir = Dir.Right;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            dir = Dir.Left;
        }

        switch (dir) {
            case Dir.Forward:
                if (isWild) {

                }
                else {
                    side.transform.localPosition = new Vector3(0.728f, -0.5601841f, -0.123f);
                }
                animatorForward.SetBool("walk", Mathf.Abs(rb.velocity.z) > 0.01f);

                forward.SetActive(true);
                side.SetActive(false);
                backward.SetActive(false);
            break;

            case Dir.Backward:
                if (isWild) {
                    backward.transform.localPosition = new Vector3(0.74f, 0.903f, -0.069f);
                }
                else {
                    backward.transform.localPosition = new Vector3(0.389f, -0.5601841f, 0.106f);
                }
                forward.SetActive(false);
                side.SetActive(false);
                backward.SetActive(true);
            break;

            case Dir.Right:
                if (isWild) {
                    side.transform.localPosition = new Vector3(0.614f, 0.903f, 0.136f);
                }
                else {
                    side.transform.localPosition = new Vector3(0.389f, -0.5601841f, 0.106f);
                }

                side.transform.localScale = new Vector3(0.3443782f, 0.3443782f, 0.3443782f);
                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;

            case Dir.Left:
                if (isWild) {
                    side.transform.localPosition = new Vector3(-0.606f, 0.903f, 0.134f);
                }
                else {
                    side.transform.localPosition = new Vector3(-0.44f, 0, 0.08f);
                }
                
                side.transform.localScale = new Vector3(-0.3443782f, 0.3443782f, 0.3443782f);
                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;
        }

        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
    }
}
