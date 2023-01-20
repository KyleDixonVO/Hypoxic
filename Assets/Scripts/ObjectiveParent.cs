using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveParent : MonoBehaviour
{
    public static ObjectiveParent objectiveParent;

    private void Awake()
    {
        if (objectiveParent == null)
        {
            objectiveParent = this;
            DontDestroyOnLoad(this);
        }
        else if (objectiveParent != null && objectiveParent != this)
        {
            Destroy(this);
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
