using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Angel : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isBusy;

    Animator anim;
    AudioSource audioSource;

    AngelGenerator ag;
    Ground ground;

    public AudioClip drop;
    public AudioClip replace;

    public AudioClip[] multiDropClips;

    // Start is called before the first frame update
    void OnEnable()
    {
        ag = AngelGenerator.Instance;
        ground = Ground.Instance;

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isBusy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReplaceTile(int dropClip=-1)
    {
        if(dropClip == -1){
            audioSource.clip = drop;
        } else {
            audioSource.clip = multiDropClips[dropClip];
        }

        StartCoroutine(_ReplaceTile());
    }

    IEnumerator _ReplaceTile()
    {
        isBusy = true;

        int replaceTileIndex = ground.GetRandomEmptyTile();
        if(replaceTileIndex >= 0){
            float targetHeight = ag.angelDropHeight.position.y;
            transform.position = new Vector3(ground.GetTileX(replaceTileIndex), transform.position.y, transform.position.z);
            Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);

            audioSource.Play();

            while(Mathf.Abs(transform.position.y - targetHeight) > 0.01f){
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                yield return null;
            }

            if(audioSource.clip == drop){
                audioSource.clip = replace;
                audioSource.Play();
            }

            ground.FillEmptyTile(replaceTileIndex);
            SetState(false);

            while(anim.GetCurrentAnimatorStateInfo(0).IsName("AngelWithTile")){
                yield return null;
            }

            targetHeight = ag.transform.position.y;
            targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            while(Mathf.Abs(transform.position.y - targetHeight) > 0.01f){
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed / 1.5f);
                yield return null;
            }

            SetState(true);
        }

        isBusy = false;
        yield return null;
    }

    public void SetState(bool hasTile)
    {
        anim.SetTrigger("Change");
        anim.SetBool("HasTile", hasTile);
    }
}
