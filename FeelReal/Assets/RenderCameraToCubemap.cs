using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraToCubemap : MonoBehaviour
{
    public Cubemap cubemap;
    public Camera camera;
    public RenderTexture rt;

    [System.Obsolete]
    private void Start()
    {
        camera.RenderToCubemap(rt);
        camera.RenderToCubemap(cubemap);

        rt.isCubemap = true;
    }
    void LateUpdate()
    {
        
    }
}