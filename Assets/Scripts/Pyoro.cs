using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pyoro : MonoBehaviour
{

    public float moveSpeed = 5f;

    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    bool isFiring;
    Vector2 inputVector;
    float facing = -1;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        isFiring = false;
        DoMovement();
    }

    public void DoMovement()
    {
        RaycastHit2D hit;
        if(inputVector.magnitude > 0){
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
            sr.flipX = facing > 0;
        }

        inputVector = moveSpeed * tempVector * Time.fixedDeltaTime;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if(isFiring){
            return;
        }

        isFiring = true;
        Debug.Log("Shoot!");
    }
}
