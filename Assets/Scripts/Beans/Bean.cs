using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bean : MonoBehaviour
{
    public enum BeanType {
        Green,
        Pink,
        Special
    }
    public BeanType beanType;

    public float fallSpeed;

    public AudioClip collectClip;
    public AudioClip explodeClip;

    public bool isCaught;

    bool isDead;

    Animator anim;
    AudioSource audioSource;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(isCaught){
            transform.localScale *= 0.95f;
            rb.simulated = false;
        } else if(!isDead){
            transform.Translate(-Vector3.up * fallSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Pyoro>().Die();
            StartCoroutine(Explode());
            isDead = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(isDead || isCaught){
            return;
        }

        if(other.gameObject.tag == "Tile"){
            Destroy(other.gameObject);
            StartCoroutine(Explode());
        } else if(other.gameObject.tag == "BeanDeathZone"){
            Destroy(gameObject);
        }
        isDead = true;
    }

    IEnumerator Explode(){
        rb.simulated = false;

        audioSource.Stop();
        audioSource.clip = explodeClip;
        audioSource.Play();

        anim.SetTrigger("BeanExplode");
        while(anim.GetCurrentAnimatorStateInfo(0).IsName("BeanExplode") || audioSource.isPlaying){
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetBeanType(string newBeanType)
    {
        switch(newBeanType.ToLower()){
            case "green":
                beanType = BeanType.Green;
                anim.SetInteger("BeanColor", 0);
                break;
            case "pink":
                beanType = BeanType.Pink;
                anim.SetInteger("BeanColor", 1);
                break;
            case "special":
                beanType = BeanType.Special;
                anim.SetInteger("BeanColor", 2);
                break;
        }
        anim.SetTrigger("BeanColorChange");
    }

    public void Activate()
    {
        switch(beanType){
            case BeanType.Pink:
                break;
            case BeanType.Special:
                break;
        }
    }
}
