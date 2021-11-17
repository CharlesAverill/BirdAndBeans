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
    const int minGenerationRate = 12;
    int frameCount;

    public Transform leftBound;
    public Transform rightBound;

    public List<Bean> beans;

    public bool generateBeans = false;
    public bool destroying = false;

    public Transform[] pointBounds = new Transform[5];

    GameController gc;

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
        gc = GameController.Instance;

        Reset();
    }

    public void Reset()
    {
        DestroyAllBeans(false);
        UpdateGenerationRate();
        frameCount = (int)generationRate;
        generateBeans = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(generateBeans && !destroying){
            UpdateGenerationRate();

            if(frameCount++ > generationRate){
                frameCount = 0;

                float xCoord = Random.Range(leftBound.position.x, rightBound.position.x);
                Bean newBean = Instantiate(beanPrefab, new Vector3(xCoord, leftBound.position.y, leftBound.position.z), Quaternion.identity).GetComponent<Bean>();
                // Set bean color with distribution {green = .9, pink = .08, special = .02}
                int r = Random.Range(1, 101);
                newBean.SetBeanType(r < 92 ? "green" : (r < 99 ? "pink" : "special"));

                newBean.fallSpeed = Bean.minFallSpeed + (Bean.maxFallSpeed - Bean.minFallSpeed) * (1 - generationRate / maxGenerationRate) + Random.Range(-0.5f, 0.5f);

                if(beans.Count == 0){
                    newBean.fallSpeed = Bean.minFallSpeed;
                    newBean.SetBeanType("green");
                }

                newBean.transform.parent = transform;
                beans.Add(newBean);
            }
        }
    }

    void UpdateGenerationRate()
    {
        generationRate = Mathf.Max(maxGenerationRate - 1f * gc.timePassed, 10);
    }

    public void DestroyAllBeans(bool playSound=true, bool addPoints=false)
    {
        StartCoroutine(_DestroyAllBeansEnumerator(playSound, addPoints));
    }

    IEnumerator _DestroyAllBeansEnumerator(bool playSound, bool addPoints)
    {
        destroying = true;

        for(int i = 0; i < beans.Count; i++){
            if(beans[i] != null){
                if(addPoints){
                    beans[i].AddPoints();
                }
                beans[i].Explode(playSound);
                yield return null;
            }
        }

        beans = new List<Bean>();
        gc.UpdateTime(-1f * gc.timePassed / 3f);
        destroying = false;
    }

    public void AddPoints(Vector3 position)
    {
        int numPoints = 0;

        if(position.y >= pointBounds[0].position.y){
            numPoints = 1000;
        } else if(position.y >= pointBounds[1].position.y){
            numPoints = 300;
        } else if(position.y >= pointBounds[2].position.y){
            numPoints = 100;
        } else if(position.y >= pointBounds[3].position.y){
            numPoints = 50;
        } else if(position.y >= pointBounds[4].position.y){
            numPoints = 10;
        }

        gc.AddPoints(numPoints, position);
    }
}
