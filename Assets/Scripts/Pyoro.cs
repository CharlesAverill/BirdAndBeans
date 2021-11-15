using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pyoro : MonoBehaviour
{

    public float moveSpeed = 5f;

    Tongue tongue;

    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    Vector2 inputVector;
    float facing = -1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        tongue = GetComponentInChildren<Tongue>();

        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        if(!tongue.isLaunching){
            anim.SetBool("isLaunching", false);
            DoMovement();
        }
    }

    public void DoMovement()
    {
        RaycastHit2D hit;
        if(inputVector.magnitude > 0){
            transform.localScale = new Vector3(-1f * facing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            tongue.Flip(-1f * facing);

            hit = Physics2D.Raycast(transform.position + new Vector3(facing * sr.size.x / 2.5f, 0f, 0f), -Vector2.up, 1f);
            if(hit.collider != null){
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 tempVector = context.ReadValue<Vector2>();
        if(tempVector.x != 0){
            facing = tempVector.x;
        }

        inputVector = moveSpeed * tempVector * Time.fixedDeltaTime;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if(tongue.isLaunching){
            return;
        }

        tongue.Launch();

        anim.SetBool("isLaunching", true);
    }

    public void Die()
    {
        
    }
}
