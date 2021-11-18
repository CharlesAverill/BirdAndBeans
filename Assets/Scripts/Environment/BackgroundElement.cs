using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    public Sprite[] sprites;
    int spriteIndex = 0;

    public SpriteRenderer from;
    public SpriteRenderer to;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        spriteIndex = 0;
        from.sprite = sprites[0];

        from.color = Color.white;
        to.color = new Color(1f, 1f, 1f, 0f);
    }

    public void Next()
    {
        StartCoroutine(_TransitionSprites());
    }

    IEnumerator _TransitionSprites()
    {
        to.sprite = sprites[++spriteIndex];

        Color fromColor = Color.white;
        Color toColor = new Color(1f, 1f, 1f, 0f);

        float steps = 30;
        for(float i = 0; i < steps; i += 1){
            fromColor.a = 1f - (i / steps);
            toColor.a = (i / steps);

            from.color = fromColor;
            to.color = toColor;

            yield return new WaitForSeconds(1f / steps);
        }

        from.color = new Color(1f, 1f, 1f, 0f);
        to.color = Color.white;

        from.sprite = to.sprite;

        from.color = Color.white;
        to.color = new Color(1f, 1f, 1f, 0f);
    }
}
