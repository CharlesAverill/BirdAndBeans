using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public float timePassed { get; private set; }

    Ground ground;
    Pyoro pyoro;
    BeanGenerator beanGen;
    public DigitalRuby.SoundManagerNamespace.BGMusic bgMusic;

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
}
