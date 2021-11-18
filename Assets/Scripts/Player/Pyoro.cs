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
    static Pyoro _instance;
    public static Pyoro Instance { get { return _instance; } }

    public float moveSpeed = 5f;

    Menu menu;

    Tongue tongue;

    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    Vector2 inputVector;
    float facing = -1;

    public bool isDead;
    public bool doneDying;

    public bool acceptInput = true;

    Transform originalTransform;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        originalTransform = new GameObject("OriginalPyoroTransform").transform;
        originalTransform.position = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        tongue = GetComponentInChildren<Tongue>();
        menu = Menu.Instance;
    }

    public void Reset()
    {
        acceptInput = false;
        isDead = false;
        doneDying = false;

        transform.position = originalTransform.position;

        anim.SetBool("isWalking", false);
        anim.SetTrigger("SetIdle");

        inputVector = Vector2.zero;

        boxCollider.enabled = true;
    }

    void FixedUpdate()
    {
        if(!tongue.isLaunching && !isDead){
            anim.SetBool("isLaunching", false);
            DoMovement();
        }
    }

    public void DoMovement()
    {
        bool playWalkAnimation = false;

        if(inputVector.magnitude > 0){
            transform.localScale = new Vector3(-1f * facing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            tongue.Flip(-1f * facing);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(facing * sr.size.x / 2.5f, 0f, 0f), -Vector2.up, 1f);
            if(hit.collider != null){
                playWalkAnimation = true;
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector);
            }
        }

        if(playWalkAnimation){
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("SetIdle");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(!acceptInput || isDead){
            return;
        }

        Vector2 tempVector = context.ReadValue<Vector2>();
        if(tempVector.x != 0){
            facing = tempVector.x;
        }

        inputVector = moveSpeed * tempVector * Time.fixedDeltaTime;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if(tongue.isLaunching || isDead || !acceptInput || !context.started){
            return;
        }

        //inputVector = Vector2.zero;
        tongue.Launch();

        anim.SetBool("isWalking", false);
        anim.SetBool("isLaunching", true);
    }

    public void Die()
    {
        if(!isDead){
            StartCoroutine(DieAnimation());
        }
    }

    IEnumerator DieAnimation()
    {
        isDead = true;
        anim.SetTrigger("die");
        acceptInput = false;

        menu.ShowGameOverScreen();

        float targetDestroyHeight = transform.position.y - sr.size.y * 3f;

        // Bounce him before he falls down
        float yVelocity = 7f;
        while(transform.position.y > targetDestroyHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + yVelocity * Time.deltaTime, transform.position.z);
            yVelocity += -9.8f * Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = false;

        //Destroy(gameObject);
        doneDying = true;
    }
}
