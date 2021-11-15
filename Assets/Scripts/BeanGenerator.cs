using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanGenerator : MonoBehaviour
{
    public GameObject beanPrefab;

    // Number of frames between bean generation
    public int generationRate = 120;
    int maxGenerationRate;
    const int minGenerationRate = 6;
    int frameCount;
    float timePassed;

    public Transform leftBound;
    public Transform rightBound;

    public List<Bean> beans;

    public bool generateBeans = false;

    // Start is called before the first frame update
    void Start()
    {
        frameCount = generationRate;
        maxGenerationRate = generationRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(generateBeans){
            timePassed += Time.deltaTime;

            UpdateGenerationRate();

            if(frameCount++ > generationRate){
                frameCount = frameCount % generationRate;

                float xCoord = Random.Range(leftBound.position.x, rightBound.position.x);
                Bean newBean = Instantiate(beanPrefab, new Vector3(xCoord, leftBound.position.y, leftBound.position.z), Quaternion.identity).GetComponent<Bean>();

                newBean.transform.parent = transform;
                beans.Add(newBean);
            }
        }
    }

    // Use a combo of sine and logistic functions to decrease the generation rate over time with randomness
    void UpdateGenerationRate()
    {
        float generationRateDiff = (maxGenerationRate - minGenerationRate);
        float logistic = generationRateDiff / (1f + Mathf.Exp(0.08f * (timePassed - generationRateDiff))) + minGenerationRate;
        float sine = 10f * Mathf.Sin(timePassed / 10f);

        generationRate =  (int)Mathf.Max(logistic + sine + Random.Range(-15f, 15f), minGenerationRate);
    }
}
