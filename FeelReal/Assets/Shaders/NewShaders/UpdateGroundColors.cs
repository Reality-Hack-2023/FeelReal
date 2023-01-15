using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGroundColors : MonoBehaviour
{
    public Material material;
    public Renderer rend;

    public Skybox skybox;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        //When the enviroment change is triggered 

    }
}
