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

    CanvasGroup mainMenuCG;

    GameController gc;
    Pyoro pyoro;
    BeanGenerator beanGen;

    public CanvasGroup gameOverScreenCG;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuCG = GetComponent<CanvasGroup>();

        pyoro = Pyoro.Instance;
        beanGen = BeanGenerator.Instance;
        gc = GameController.Instance;

        pyoro.acceptInput = false;

        onMainMenu = true;

        startMusic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(startMusic){
            gc.bgMusic.PlayMenuMusic(0.01f);
            startMusic = false;
        }
    }

    public void Start(InputAction.CallbackContext context)
    {
        if(onMainMenu){
            hideGameOver = true;
            gameOverScreenCG.alpha = 0f;
            gc.Reset();

            StartCoroutine(TitleFade(false));
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
        StartCoroutine(GameOverFade(false));
        StartCoroutine(TitleFade(true));
    }

    public void ShowGameOverScreen()
    {
        hideGameOver = false;
        gc.bgMusic.PlayGameOver();
        StartCoroutine(GameOverFade(true));
    }

    IEnumerator TitleFade(bool fadeIn)
    {
        while(beanGen.destroying){
            yield return null;
        }

        if(fadeIn){
            gc.bgMusic.PlayMenuMusic(0.01f);

            while(mainMenuCG.alpha < 1){
                mainMenuCG.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }

            onMainMenu = true;
            gc.Reset();
            transitioningToMenu = false;
        } else {
            gc.bgMusic.PlayGameMusic(1);
            onMainMenu = false;

            while(mainMenuCG.alpha > 0){
                mainMenuCG.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }

            beanGen.generateBeans = true;
        }

        gameOverScreenCG.alpha = 0f;
        if(!fadeIn){

            hideGameOver = true;
        }

        pyoro.acceptInput = !fadeIn;
    }

    IEnumerator GameOverFade(bool fadeIn)
    {
        while(beanGen.destroying){
            yield return null;
        }

        if(fadeIn){
            while(gameOverScreenCG.alpha < 1 && !hideGameOver){
                gameOverScreenCG.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }
        } else {
            while(gameOverScreenCG.alpha > 0){
                gameOverScreenCG.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
