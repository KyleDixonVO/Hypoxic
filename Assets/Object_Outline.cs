using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Outline : MonoBehaviour
{
    public Outline _outline;
    public Outline _outline2;
    // Start is called before the first frame update
    void Start()
    {
        _outline.enabled = true;
        _outline2.enabled = true;        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
