using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public float timePassed { get; private set; }

    int score;
    int highScore;

    Ground ground;
    Pyoro pyoro;
    BeanGenerator beanGen;
    public DigitalRuby.SoundManagerNamespace.BGMusic bgMusic;

    public Text scoreText;
    public Text highScoreText;

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

        score = 0;

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

        score = 0;
        scoreText.text = "000000";
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
        score = Mathf.Min(score + value, 999999);
        highScore = Mathf.Max(score, highScore);

        scoreText.text = score.ToString("000000");
        highScoreText.text = highScore.ToString("000000");

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
