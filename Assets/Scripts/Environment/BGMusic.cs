using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.SoundManagerNamespace
{
    public class BGMusic : MonoBehaviour
    {
        static BGMusic _instance;
        public static BGMusic Instance { get { return _instance; } }

        public AudioSource mainMenu;
        public AudioSource theme1;
        public AudioSource theme2;
        public AudioSource theme3;
        public AudioSource sepia;
        public AudioSource gameOver;

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

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayMenuMusic(float transitionTime)
        {
            mainMenu.PlayLoopingMusicManaged(1.0f, transitionTime, false);
        }

        public void PlayTheme1()
        {
            theme1.PlayLoopingMusicManaged(1.0f, 1.0f, false);
        }

        public void PlayGameOver()
        {
            gameOver.PlayLoopingMusicManaged(.8f, 0f, false);
        }
    }
}
