using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FogEffect : MonoBehaviour
{
    public Material _mat;
    public Color _fogColor;
    public float _depthStart;
    public float _depthDistance;
    public bool effectActive;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        effectActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        //_mat.SetColor("_FogColor", _fogColor);
        //_mat.SetFloat("_DepthStart", _depthStart);
        //_mat.SetFloat("_DepthDistance", _depthDistance);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!effectActive)
        {
            //RenderSettings.fog = false;
            Graphics.Blit(source, destination);
            return;
        }

        RenderSettings.fog = true;
        Graphics.Blit(source, destination);
        //Graphics.Blit(source, destination, _mat);
    }
}
