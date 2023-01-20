using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSingleton : MonoBehaviour
{
    public static PipeSingleton pipe1;
    public static PipeSingleton pipe2;
    public static PipeSingleton pipe3;

    private void Awake()
    {
        if (pipe1 == null && pipe2 != this && pipe3 != this)
        {
            pipe1 = this;
            DontDestroyOnLoad(this);
        }
        else if (pipe2 == null && pipe1 != this && pipe3 != this)
        {
            pipe2 = this;
            DontDestroyOnLoad(this);
        }
        else if (pipe3 == null && pipe1 != this && pipe3 != this)
        {
            pipe3 = this;
            DontDestroyOnLoad(this);
        }
        else if (pipe3 != null && pipe3 != this && pipe1 != this && pipe2 != this)
        {
            Destroy(this.gameObject);
        }
        else if (pipe2 != null && pipe2 != this && pipe1 != this && pipe3 != this)
        {
            Destroy(this.gameObject);
        }
        else if (pipe1 != null && pipe1 != this && pipe2 != this && pipe3 != this)
        {
            Destroy(this.gameObject);
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
}
