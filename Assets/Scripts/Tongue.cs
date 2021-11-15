﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Tongue : MonoBehaviour
{
    public float speed = 5f;

    public bool isLaunching;
    bool isRetracting;

    CircleCollider2D circleCollider;
    Rigidbody2D rb;

    LineRenderer lr;
    Transform startPoint;
    Transform endPoint;

    Bean caughtBean;

    float flip = 1f;

    // For calculating y = mx + b
    float lineSlope;
    float lineOffset;

    float newX;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;

        GameObject temp1 = new GameObject("Tongue Start");
        startPoint = temp1.transform;
        GameObject temp2 = new GameObject("Tongue End");
        endPoint = temp2.transform;

        startPoint.localPosition = lr.GetPosition(0);
        endPoint.localPosition = lr.GetPosition(1);

        startPoint.transform.parent = transform;
        endPoint.transform.parent = transform;

        lineSlope = -0.7f; //(endPoint.y - startPoint.y) / (endPoint.x - startPoint.x);
        lineOffset = startPoint.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLaunching)
        {
            MoveTongueEnd();
        }
    }

    float getTongueY(float x)
    {
        return lineSlope * x + lineOffset;
    }

    void MoveTongueEnd()
    {
        if(isRetracting)
        {
            newX = endPoint.localPosition.x + (Time.deltaTime * speed);
        } else {
            newX = endPoint.localPosition.x - (Time.deltaTime * speed);
        }

        endPoint.localPosition = new Vector3(newX, getTongueY(newX), startPoint.localPosition.z);
        lr.SetPosition(1, endPoint.localPosition);

        circleCollider.offset = new Vector2(endPoint.localPosition.x, endPoint.localPosition.y);
    }

    public void Launch()
    {
        isLaunching = true;
        isRetracting = false;

        rb.simulated = true;

        StartCoroutine(launchEnumerator());
    }

    IEnumerator launchEnumerator()
    {
        while(!isRetracting)
        {
            yield return null;
        }

        rb.simulated = false;

        while(isRetracting)
        {
            if(startPoint.localPosition.x < endPoint.localPosition.x){
                isRetracting = false;
            }
            yield return null;
        }

        isLaunching = false;
        isRetracting = false;

        if(caughtBean != null){
            caughtBean.Activate();
            Destroy(caughtBean.gameObject);
        }

        endPoint.localPosition = startPoint.localPosition;
        lr.SetPosition(1, endPoint.localPosition);
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
        if(isLaunching){
            isRetracting = true;
        }
    }

    public void Flip(float n)
    {
        flip = n;
    }
}