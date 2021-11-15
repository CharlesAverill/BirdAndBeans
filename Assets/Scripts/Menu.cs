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

    Pyoro pyoro;
    BeanGenerator beanGen;
    public DigitalRuby.SoundManagerNamespace.BGMusic bgMusic;

    bool startMusic;

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
        cg = GetComponent<CanvasGroup>();

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
            StartCoroutine(HideAnimation());
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    IEnumerator HideAnimation()
    {
        onMainMenu = false;

        bgMusic.PlayTheme1();

        while(cg.alpha > 0){
            cg.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        pyoro.acceptInput = true;
        beanGen.generateBeans = true;
    }
}
