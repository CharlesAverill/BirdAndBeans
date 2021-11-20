using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMusic : MonoBehaviour
{
    static BGMusic _instance;
    public static BGMusic Instance { get { return _instance; } }

    public AudioMixer audioMixer;

    public AudioSource mainMenu;
    public AudioSource theme1;
    public AudioSource theme2;
    public AudioSource theme3;
    public AudioSource sepia;
    public AudioSource challenge1;
    public AudioSource challenge2;
    public AudioSource challenge3;
    public AudioSource gameOver;

    AudioSource current;

    GameController gc;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        MuteAllLevels();
    }

    // Start is called before the first frame update
    void Start()
    {
        current = mainMenu;

        gc = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopAll()
    {
        mainMenu.Stop();
        theme1.Stop();
        theme2.Stop();
        theme3.Stop();
        sepia.Stop();
        challenge1.Stop();
        challenge2.Stop();
        challenge3.Stop();
        gameOver.Stop();
    }

    public void MuteAllLevels()
    {
        theme1.volume = 0f;
        theme2.volume = 0f;
        theme3.volume = 0f;
        sepia.volume = 0f;
        challenge1.volume = 0f;
        challenge2.volume = 0f;
        challenge3.volume = 0f;
    }

    public void PlayAllLevels()
    {
        theme1.Play();
        theme2.Play();
        theme3.Play();
        sepia.Play();
    }

    public void PlayMenuMusic(float transitionTime)
    {
        MuteAllLevels();
        Transition(mainMenu, .5f, 0f);
    }

    public void PlayGameMusic(int level)
    {
        switch(level){
            case 1:
                MuteAllLevels();
                PlayAllLevels();
                Transition(theme1, .5f);
                break;
            case 2:
                Transition(theme2, .5f);
                break;
            case 3:
                Transition(theme3, .5f);
                break;
            case 4:
                sepia.Stop();
                sepia.Play();
                Transition(sepia, .8f);
                break;
            case 5:
                challenge1.Stop();
                challenge2.Stop();
                challenge3.Stop();
                challenge1.Play();
                challenge2.Play();
                challenge3.Play();
                Transition(challenge1, .8f);
                break;
            case 6:
                Transition(challenge2, .8f);
                break;
            case 7:
                Transition(challenge3, .8f);
                break;
        }
    }

    public void PlayGameOver()
    {
        StopAll();
        gameOver.volume = .8f;
        gameOver.Play();
        current = gameOver;
        //Transition(gameOver, 0.8f, 0f);
    }

    public void Transition(AudioSource to, float transitionVolume=1f, float transitionTime=1f)
    {
        StartCoroutine(_TransitionEnum(to, transitionVolume, transitionTime));
    }

    IEnumerator _TransitionEnum(AudioSource to, float transitionVolume=1f, float transitionTime=1f)
    {
        if(current == to){
            yield break;
        }

        if(to == mainMenu){
            to.Play();
        }

        current.volume = transitionVolume;
        to.volume = 0f;

        int steps = 30;
        float wait = transitionTime * 1f / (float)steps;
        float delta = transitionVolume * wait;
        for(int i = 0; i < steps; i++){
            current.volume -= delta * 2f;
            to.volume += delta;
            yield return new WaitForSeconds(wait);
        }

        current.volume = 0f;
        to.volume = transitionVolume;

        if(current == mainMenu){
            current.Stop();
        }

        current = to;
    }
}
