using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Angel : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isBusy;

    Animator anim;

    AngelGenerator ag;
    Ground ground;

    // Start is called before the first frame update
    void OnEnable()
    {
        ag = AngelGenerator.Instance;
        ground = Ground.Instance;

        anim = GetComponent<Animator>();
        isBusy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReplaceTile()
    {
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

            while(Mathf.Abs(transform.position.y - targetHeight) > 0.01f){
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                yield return null;
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
