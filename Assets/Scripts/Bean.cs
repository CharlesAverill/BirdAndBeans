using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bean : MonoBehaviour
{
    public float fallSpeed;

    public AudioClip collectClip;
    public AudioClip explodeClip;

    bool isDead;

    Animator anim;
    AudioSource audioSource;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(!isDead){
            transform.Translate(-Vector3.up * fallSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(isDead){
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
}
