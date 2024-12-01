using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{

    // Static instance of the GameManager
    private static TimeManagement instance;
    // Property to access the instance
    public static TimeManagement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeManagement>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<TimeManagement>();
                    singletonObject.name = typeof(TimeManagement).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
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

    public void PauseGame()
    {
        Time.timeScale = 0;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Pause();
        }


    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (!audio.isPlaying)
                audio.Play();
        }
    }

}
