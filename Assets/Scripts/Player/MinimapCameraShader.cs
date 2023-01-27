using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraShader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().SetReplacementShader(Shader.Find("Unlit/Color"), "RenderType");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
