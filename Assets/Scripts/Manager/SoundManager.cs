using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Static instance of the GameManager
    private static SoundManager instance;
    // Property to access the instance
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<SoundManager>();
                    singletonObject.name = typeof(SoundManager).ToString() + " (Singleton)";
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
}
