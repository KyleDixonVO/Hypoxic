using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float PerlinNoise(float x, float y)
    {
        return ((Mathf.PerlinNoise(x, y)));
    }

    public Vector3 PerlinHeadBob(float mainVar, float varY, float frequency, float amplitude, float lowerBobBound, float upperBobBound)
    {
        Vector3 deltaPos;
        deltaPos.x = 0.0f;
        deltaPos.y = Mathf.Clamp(Mathf.Sin(mainVar) + (PerlinNoise(mainVar * frequency, varY) * amplitude), lowerBobBound, upperBobBound);
        deltaPos.z = 0.0f;
        return deltaPos;
    }
}

