using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{

    public Sprite[] backgroundSprites;
    public int backgroundIndex = -1;

    SpriteRenderer sr;

    int canaryIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Pick a random background
        if(backgroundIndex < 0) {
            backgroundIndex = Random.Range(0, backgroundSprites.Length + 1);
        }

        sr = GetComponent<SpriteRenderer>();

        // Initialize the background
        SetBackground(backgroundIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for manual background changes
        if(backgroundIndex != canaryIndex){
            SetBackground(backgroundIndex);
        }
    }

    public void SetBackground(int index){
        // Check for out-of-bounds
        canaryIndex = backgroundIndex = Mathf.Abs(index % backgroundSprites.Length);

        // Update SpriteRenderer
        sr.sprite = backgroundSprites[backgroundIndex];
    }
}
