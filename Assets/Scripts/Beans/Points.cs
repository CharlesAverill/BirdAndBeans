using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Points : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetValue(int value)
    {
        Hide();
        anim.SetInteger("Value", value);
    }

    public void Hide()
    {
        sr.enabled = false;
    }

    public void Show()
    {
        sr.enabled = true;
    }
}
