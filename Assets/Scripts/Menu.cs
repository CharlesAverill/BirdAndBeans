using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    static Menu _instance;
    public static Menu Instance { get { return _instance; } }

    public float fadeSpeed = 5f;

    bool onMainMenu;

    CanvasGroup cg;

    Ground ground;
    Pyoro pyoro;
    BeanGenerator beanGen;
    public DigitalRuby.SoundManagerNamespace.BGMusic bgMusic;

    public CanvasGroup gameOverScreenCg;

    bool startMusic;
    bool transitioningToMenu;
    bool hideGameOver;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();

        ground = Ground.Instance;
        pyoro = Pyoro.Instance;
        beanGen = BeanGenerator.Instance;
        bgMusic = DigitalRuby.SoundManagerNamespace.BGMusic.Instance;

        pyoro.acceptInput = false;

        onMainMenu = true;

        startMusic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(startMusic){
            bgMusic.PlayMenuMusic(0.01f);
            startMusic = false;
        }
    }

    public void Start(InputAction.CallbackContext context)
    {
        if(onMainMenu){
            hideGameOver = true;
            beanGen.Reset();
            pyoro.Reset();
            ground.Reset();
            gameOverScreenCg.alpha = 0f;
            StartCoroutine(Fade(false, cg));
        } else if(pyoro.isDead && pyoro.doneDying && !transitioningToMenu){
            transitioningToMenu = true;
            ShowMainMenu();
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    void ShowMainMenu()
    {
        StartCoroutine(Fade(false, gameOverScreenCg));
        StartCoroutine(Fade(true, cg));
    }

    public void ShowGameOverScreen()
    {
        hideGameOver = false;
        StartCoroutine(Fade(true, gameOverScreenCg));
    }

    IEnumerator Fade(bool fadeIn, CanvasGroup fadeCG)
    {
        while(beanGen.destroying){
            yield return null;
        }

        if(fadeIn){
            if(fadeCG == cg){
                bgMusic.PlayMenuMusic(0.01f);
            }

            while(fadeCG.alpha < 1 && !(fadeCG == gameOverScreenCg && hideGameOver)){
                fadeCG.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }

            if(fadeCG == cg){
                onMainMenu = true;
                beanGen.Reset();
                pyoro.Reset();
                ground.Reset();
                transitioningToMenu = false;
            }
        } else {
            bgMusic.PlayTheme1();
            onMainMenu = false;
            while(fadeCG.alpha > 0){
                fadeCG.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
        }

        if(fadeCG == cg){
            gameOverScreenCg.alpha = 0f;
            if(!fadeIn){
                beanGen.generateBeans = true;
                hideGameOver = true;
            }
        }

        pyoro.acceptInput = !fadeIn && fadeCG == cg;
    }
}
