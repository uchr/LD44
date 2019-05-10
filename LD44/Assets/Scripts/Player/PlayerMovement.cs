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
    public float fastSpeed = 20.0f;
    public float normalSpeed = 7.5f;

    public GameObject forward;
    public GameObject side;
    public GameObject backward;

    public bool isWild = false;

    private Rigidbody rb;
    private Animator animatorForward;
    private Animator animatorSide;
    private Animator animatorBackward;

    public bool idleEnabled = true;
    public Dir dir;
    private float speed;

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
        if (MainQuest.instance.isEnd)
            return;

        if (idleEnabled)
            dir = Dir.None;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            dir = Dir.Forward;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            dir = Dir.Backward;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            dir = Dir.Right;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            dir = Dir.Left;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            speed = fastSpeed;
        else 
            speed = normalSpeed;

        if (dir != Dir.None) {
            if (isWild)
                Stem.SoundManager.Play("StepGrass");
            else 
                Stem.SoundManager.Play("StepMetal");
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
                    backward.transform.localPosition = new Vector3(0.938f, 1.256f, 0.059f);
                }
                else {
                    backward.transform.localPosition = new Vector3(1.06f, 0f, -0.24f);
                }
                forward.SetActive(false);
                side.SetActive(false);
                backward.SetActive(true);
            break;

            case Dir.Right:
                if (isWild) {
                    side.transform.localPosition = new Vector3(1.27f, 1.744f, 0.879f);
                    side.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                }
                else {
                    side.transform.localPosition = new Vector3(1.3f, 0, 0.9f);
                    side.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;

            case Dir.Left:
                if (isWild) {
                    side.transform.localPosition = new Vector3(-1.27f, 1.744f, 0.756f);
                    side.transform.localScale = new Vector3(-0.45f, 0.45f, 0.45f);
                }
                else {
                    side.transform.localPosition = new Vector3(-1.3f, 0, 0.9f);
                    side.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }

                forward.SetActive(false);
                side.SetActive(true);
                backward.SetActive(false);
            break;
        }

        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        rb.velocity = new Vector3(inputDir.x * speed, rb.velocity.y, inputDir.z * speed);
    }
}
