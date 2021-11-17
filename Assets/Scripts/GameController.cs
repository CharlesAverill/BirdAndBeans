using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public float timePassed { get; private set; }

    int points;

    Ground ground;
    Pyoro pyoro;
    BeanGenerator beanGen;
    public DigitalRuby.SoundManagerNamespace.BGMusic bgMusic;

    public GameObject pointsPrefab;

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
        timePassed = 0f;

        ground = Ground.Instance;
        pyoro = Pyoro.Instance;
        beanGen = BeanGenerator.Instance;
    }

    public void Reset()
    {
        ResetTime();
        beanGen.Reset();
        pyoro.Reset();
        ground.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
    }

    public void ResetTime()
    {
        timePassed = 0f;
    }

    public void UpdateTime(float delta)
    {
        timePassed += delta;
    }

    public void AddPoints(int value, Vector3 position)
    {
        points += value;

        StartCoroutine(_ShowPointsEnumerator(value, position));
    }

    IEnumerator _ShowPointsEnumerator(int value, Vector3 position)
    {
        Points showPoints = Instantiate(pointsPrefab, position + new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Points>();
        showPoints.SetValue(value);
        showPoints.Show();

        yield return new WaitForSeconds(0.6f + (value > 100 ? 0.4f : 0f));

        Destroy(showPoints.gameObject);
    }
}
