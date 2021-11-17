using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelGenerator : MonoBehaviour
{
    static AngelGenerator _instance;
    public static AngelGenerator Instance { get { return _instance; } }

    public GameObject angelPrefab;

    public int numAngels = 10;
    public float timeBetweenAllAngels = 2f;
    float timePerAngel;
    Angel[] angels;

    public Transform angelDropHeight;

    Ground ground;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {
        angels = new Angel[numAngels];
        for(int i = 0; i < numAngels; i++){
            angels[i] = Instantiate(angelPrefab, transform.position, Quaternion.identity).GetComponent<Angel>();
            angels[i].transform.parent = transform;
            angels[i].SetState(true);
        }

        timePerAngel = timeBetweenAllAngels / (float)numAngels;

        ground = Ground.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendAngel()
    {
        int i = GetIdleAngelIndex();
        if(i >= 0){
            angels[i].ReplaceTile();
        } else {
            throw new Exception("No Idle Angels!");
        }
    }

    public void SendAllAngels()
    {
        StartCoroutine(_SendAllAngelsCoroutine());
    }

    IEnumerator _SendAllAngelsCoroutine()
    {
        int max = Mathf.Min(numAngels, ground.NumEmptyTiles());
        for(int i = 0; i < max; i++){
            SendAngel();
            yield return new WaitForSeconds(timePerAngel);
        }
    }

    int GetIdleAngelIndex()
    {
        for(int i = 0; i < numAngels; i++){
            if(!angels[i].isBusy){
                return i;
            }
        }

        // This should never happen
        return -1;
    }
}
