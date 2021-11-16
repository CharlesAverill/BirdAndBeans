using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanGenerator : MonoBehaviour
{
    static BeanGenerator _instance;
    public static BeanGenerator Instance { get { return _instance; } }

    public GameObject beanPrefab;

    // Number of frames between bean generation
    public float generationRate = 120;
    float maxGenerationRate;
    const int minGenerationRate = 6;
    int frameCount;
    float timePassed;

    public Transform leftBound;
    public Transform rightBound;

    public List<Bean> beans;

    public bool generateBeans = false;

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
        frameCount = (int)generationRate;
        maxGenerationRate = generationRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(generateBeans){
            timePassed += Time.deltaTime;
            if(timePassed > 15f && (int)timePassed % 15 == 0){
                Debug.Log(timePassed + " " + generationRate);
            }

            UpdateGenerationRate();

            if(frameCount++ > generationRate){
                frameCount = 0;

                float xCoord = Random.Range(leftBound.position.x, rightBound.position.x);
                Bean newBean = Instantiate(beanPrefab, new Vector3(xCoord, leftBound.position.y, leftBound.position.z), Quaternion.identity).GetComponent<Bean>();
                // Set bean color with distribution {green = .9, pink = .08, special = .02}
                int r = Random.Range(1, 101);
                newBean.SetBeanType(r < 90 ? "green" : (r < 98 ? "pink" : "special"));

                newBean.transform.parent = transform;
                beans.Add(newBean);
            }
        }
    }

    void UpdateGenerationRate()
    {
        generationRate = Mathf.Max(maxGenerationRate - 2f * timePassed, 10);
    }
}
