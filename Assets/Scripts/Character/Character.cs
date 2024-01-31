using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 0.0f;
    public float lateralMovement = 2.0f;
    public float jumpMovement = 400.0f;

    public Transform groundCheck;

    private Animator animator;
    private Rigidbody2D rigidbody2d;

    public bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator> ();
        rigidbody2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast (transform.position,
        groundCheck.position,
        LayerMask.GetMask ("Ground"));
        if (grounded && Input.GetButtonDown ("Jump"))
            rigidbody2d.AddForce (Vector2.up * jumpMovement);
        if (grounded)
            animator.SetTrigger ("Grounded");
        else
            animator.SetTrigger ("Jump");
            Speed = lateralMovement * Input.GetAxis ("Horizontal");
            transform.Translate (Vector2.right * Speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(Speed));
        if (Speed < 0)
            transform.localScale = new Vector3 (-1, 1, 1);
        else
            transform.localScale = new Vector3 (1, 1, 1);
    }
}
