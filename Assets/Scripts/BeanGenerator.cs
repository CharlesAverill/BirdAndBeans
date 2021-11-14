using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanGenerator : MonoBehaviour
{
    public GameObject beanPrefab;

    // Number of frames between bean generation
    public int generationRate = 120;
    int frameCount;

    public Transform leftBound;
    public Transform rightBound;

    public List<Bean> beans;

    // Start is called before the first frame update
    void Start()
    {
        frameCount = generationRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(frameCount++ > generationRate){
            frameCount = frameCount % generationRate;

            float xCoord = Random.Range(leftBound.position.x, rightBound.position.x);
            Bean newBean = Instantiate(beanPrefab, new Vector3(xCoord, leftBound.position.y, leftBound.position.z), Quaternion.identity).GetComponent<Bean>();

            newBean.transform.parent = transform;
            beans.Add(newBean);
        }
    }
}
