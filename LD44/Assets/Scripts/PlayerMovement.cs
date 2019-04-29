using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir {
    Forward,
    Backward,
    Right,
    Left,
    None
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
        dir = Dir.None;
        if (Input.GetKey(KeyCode.S)) {
            dir = Dir.Forward;
        }
        if (Input.GetKey(KeyCode.W)) {
            dir = Dir.Backward;
        }
        if (Input.GetKey(KeyCode.D)) {
            dir = Dir.Right;
        }
        if (Input.GetKey(KeyCode.A)) {
            dir = Dir.Left;
        }

        switch (dir) {
            case Dir.None:
                animatorForward.SetBool("walk", false);
                forward.SetActive(true);
                side.SetActive(false);
                backward.SetActive(false);
                break;
            case Dir.Forward:
                animatorForward.SetBool("walk", true);

                forward.SetActive(true);
                side.SetActive(false);
                backward.SetActive(false);
            break;

            case Dir.Backward:
                if (isWild) {
                    backward.transform.localPosition = new Vector3(0.741f, 0.903f, -0.082f);
                }
                else {
                    backward.transform.localPosition = new Vector3(0.779f, 0f, -0.091f);
                }
                forward.SetActive(false);
                side.SetActive(false);
                backward.SetActive(true);
            break;

            case Dir.Right:
                if (isWild) {
                    side.transform.localPosition = new Vector3(0.796f, 1.3f, 0.435f);
                }
                else {
                    side.transform.localPosition = new Vector3(0.901f, 0f, 0.607f);
                }

                side.transform.localScale = new Vector3(0.3443782f, 0.3443782f, 0.3443782f);
                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;

            case Dir.Left:
                if (isWild) {
                    side.transform.localPosition = new Vector3(-0.844f, 1.3f, 0.435f);
                }
                else {
                    side.transform.localPosition = new Vector3(-0.836f, 0, 0.607f);
                }
                
                side.transform.localScale = new Vector3(-0.3443782f, 0.3443782f, 0.3443782f);
                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;
        }

        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        rb.velocity = new Vector3(inputDir.x * speed, rb.velocity.y, inputDir.z * speed);
    }
}
