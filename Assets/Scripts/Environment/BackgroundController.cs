using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    static BackgroundController _instance;
    public static BackgroundController Instance { get { return _instance; } }

    int currentTransition = 0;

    public BackgroundElement[] backgroundElements;
    public int[] backgroundElementsTransitionScores;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        currentTransition = 0;

        for(int i = 0; i < backgroundElements.Length; i++){
            if(backgroundElements[i] != null){
                backgroundElements[i].Reset();
            }
        }
    }

    public void UpdateBackground(int score)
    {
        score /= 1000;
        for(int i = currentTransition; i < backgroundElementsTransitionScores.Length; i++){
            if(score >= backgroundElementsTransitionScores[i] && backgroundElements[i] != null){
                backgroundElements[i].Next();
                currentTransition = i + 1;
            }
        }
    }
}
