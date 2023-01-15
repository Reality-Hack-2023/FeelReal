using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransitions : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject sphereNode;
    public GameObject containerNode;
    public GameObject pointCloud;
    public Vector3 groundPos;
    public Vector3 restPos;
    public Vector3 cloudPos;
    void Start()
    {
        containerNode.transform.position = groundPos;
        pointCloud.transform.position = cloudPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.stageState == 1)
        {
            MoveToPos(restPos, 2, 2.5f);
            if (containerNode.transform.position.y >= restPos.y - 0.15f)
            {
                sphereNode.GetComponent<ChangeShader>().ExpandOverTime(2);
            }
        }
        if (GameManager.stageState >= 3)
        {
            MoveToPos(cloudPos, 5, 0.5f);
            MovePointCloud(restPos, 5, 0.5f);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.stageState = 1;

        }
    }

    void MoveToPos(Vector3 pos, float time, float speed)
    {
        // move to pos over time lerp
        float t = time * Time.deltaTime;
        t = t * t * (3f - 2f * t)*speed;
        containerNode.transform.position = Vector3.Lerp(containerNode.transform.position, pos, t);
    }

    void MovePointCloud(Vector3 pos, float time, float speed)
    {
        float t = time * Time.deltaTime;
        t = t * t * (3f - 2f * t) * speed;
        pointCloud.transform.position = Vector3.Lerp(containerNode.transform.position, pos, t);
    }

}