using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Tongue : MonoBehaviour
{
    public float speed = 5f;
    float minSpeed;

    float launchStart;
    public bool isLaunching;
    public bool canRetract {
        get {
            return Mathf.Abs(Time.time - launchStart) > 0.2f;
        }
    }
    bool isRetracting;

    public SpriteRenderer tongueEndSprite;

    CircleCollider2D circleCollider;
    Rigidbody2D rb;

    LineRenderer lr;
    Transform startPoint;
    Transform endPoint;

    AudioSource audioSource;

    public AudioClip tongueLaunch;
    public AudioClip tongueRetract;
    public AudioClip beanCollect;

    Bean caughtBean;

    float flip = 1f;

    // For calculating y = mx + b
    float lineSlope;
    float lineOffset;

    float newX;

    Pyoro pyoro;
    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        pyoro = Pyoro.Instance;
        gc = GameController.Instance;

        rb.simulated = false;

        GameObject temp1 = new GameObject("Tongue Start");
        startPoint = temp1.transform;
        GameObject temp2 = new GameObject("Tongue End");
        endPoint = temp2.transform;

        tongueEndSprite.transform.parent = endPoint;
        tongueEndSprite.enabled = false;

        startPoint.localPosition = lr.GetPosition(0);
        endPoint.localPosition = lr.GetPosition(1);

        startPoint.transform.parent = transform;
        endPoint.transform.parent = transform;

        lineSlope = -0.8f; //(endPoint.y - startPoint.y) / (endPoint.x - startPoint.x);
        lineOffset = startPoint.localPosition.y;

        minSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(pyoro.isDead || !pyoro.acceptInput){
            endPoint.localPosition = startPoint.localPosition;
            lr.SetPosition(1, endPoint.localPosition);
            tongueEndSprite.enabled = false;

            circleCollider.offset = new Vector2(endPoint.localPosition.x, endPoint.localPosition.y);
        } else if(isLaunching)
        {
            MoveTongueEnd();
        } else {
            UpdateTongueSpeed();
        }
    }

    public void Reset()
    {
        speed = minSpeed;

        tongueEndSprite.enabled = false;
    }

    void UpdateTongueSpeed()
    {
        speed = Mathf.Min(minSpeed + gc.timePassed / 100f, 15f);
    }

    float getTongueY(float x)
    {
        return lineSlope * x + lineOffset;
    }

    void MoveTongueEnd()
    {
        if(isRetracting)
        {
            newX = endPoint.localPosition.x + (Time.deltaTime * speed * 5f);
        } else {
            newX = endPoint.localPosition.x - (Time.deltaTime * speed);
        }

        endPoint.localPosition = new Vector3(newX, getTongueY(newX), startPoint.localPosition.z);
        lr.SetPosition(1, endPoint.localPosition);

        circleCollider.offset = new Vector2(endPoint.localPosition.x, endPoint.localPosition.y);
    }

    public void Launch()
    {
        launchStart = Time.time;

        isLaunching = true;
        isRetracting = false;

        rb.simulated = true;

        StartCoroutine(launchEnumerator());
    }

    IEnumerator launchEnumerator()
    {
        tongueEndSprite.enabled = true;

        audioSource.clip = tongueLaunch;
        audioSource.Play();

        while(!isRetracting)
        {
            yield return null;
        }

        audioSource.clip = tongueRetract;
        audioSource.Play();

        tongueEndSprite.enabled = false;
        rb.simulated = false;

        while(isRetracting)
        {
            if(startPoint.localPosition.x <= endPoint.localPosition.x + 1f){
                isRetracting = false;
            }
            yield return null;
        }

        tongueEndSprite.transform.localScale = new Vector3(1f, 1f, 1f);

        audioSource.Stop();

        isLaunching = false;
        isRetracting = false;

        if(caughtBean != null){
            audioSource.clip = beanCollect;
            audioSource.Play();

            caughtBean.Activate();
            Destroy(caughtBean.gameObject);

            pyoro.Chew();
        }

        endPoint.localPosition = startPoint.localPosition;
        lr.SetPosition(1, endPoint.localPosition);
    }

    public void ForceRetract()
    {
        if(isLaunching){
            isRetracting = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(isLaunching && other.gameObject.tag != "Player"){
            caughtBean = other.gameObject.GetComponent<Bean>();
            caughtBean.transform.parent = endPoint;
            caughtBean.isCaught = true;

            isRetracting = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ForceRetract();
    }

    public void Flip(float n)
    {
        flip = n;
    }
}
