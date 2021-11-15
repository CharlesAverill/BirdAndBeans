using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    public float fadeSpeed = 5f;

    bool onMainMenu;

    CanvasGroup cg;

    Pyoro pyoro;
    BeanGenerator bg;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();

        pyoro = GameObject.FindWithTag("Player").GetComponent<Pyoro>();
        bg = GameObject.FindWithTag("BeanGenerator").GetComponent<BeanGenerator>();

        pyoro.acceptInput = false;

        onMainMenu = true;
    }

    // Update is called once per frame
    void Update()
    {

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
        while(cg.alpha > 0){
            cg.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        pyoro.acceptInput = true;
        bg.generateBeans = true;
    }
}
