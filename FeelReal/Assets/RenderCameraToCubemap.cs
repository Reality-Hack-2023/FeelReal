using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraToCubemap : MonoBehaviour
{
    public Cubemap cubemap;
    public Camera camera;
    public RenderTexture rt;

    private void Start()
    {
        camera.RenderToCubemap(rt); 
    }
    void LateUpdate()
    {
        
    }
}