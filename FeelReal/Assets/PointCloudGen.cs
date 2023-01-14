using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCloudGen : MonoBehaviour
{

    public GameObject particleCloudPrefab;
    public GameObject audioLoudnessPrefab;
    public int numPoints;
    private List<GameObject> pointClouds = new List<GameObject>();
    public float minDistance = 3;
    public int radius;

    void Start()
    {
        Vector3 vec = new Vector3(0f, 0f, 0f);
        genPointCloud(vec);
    }

    void genPointCloud(Vector3 origin)
    {
        for (int i = 0; i < numPoints; i++)
        {
            float theta = Random.Range(0f, 2f * Mathf.PI);
            float v = Random.Range(0, 1f);
            float u = Random.Range(0, 1f);
            float phi = Mathf.Acos(2f * v - 1f);
            float x = Mathf.Sqrt(1f - Mathf.Pow(u, 2)) * Mathf.Cos(theta);
            float y = u;
            float z = Mathf.Sqrt(1f - Mathf.Pow(u, 2)) * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, z) * radius;
            int runs = 0;
            while (!isValidPosition(pos))
            {
                if (++runs >= 100) break;
                float newTheta = Random.Range(0f, 2f * Mathf.PI);
                float newV = Random.Range(0, 1f);
                float newU = Random.Range(0, 1f);
                float newPhi = Mathf.Acos(2f * newV - 1f);
                float newX = Mathf.Sqrt(1f - Mathf.Pow(newU, 2)) * Mathf.Cos(newTheta);
                float newY = newU;
                float newZ = Mathf.Sqrt(1f - Mathf.Pow(newU, 2)) * Mathf.Sin(newTheta);
                Vector3 newPos = new Vector3(newX, newY, newZ) * radius;
                newPos += origin;
                pos = newPos;
            }
            if (runs >= 100)
            {
                Debug.Log("Only Generated " + i + " nodes");
                break;
            }
            GameObject pointCloud = Instantiate(particleCloudPrefab, pos, Quaternion.identity);
            if (pos.x >= 0 && pos.z >= 0)
            {
                if (pos.x >= pos.z)
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
                }
                else
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1);
                }
            }
            else if (pos.x < 0 && pos.z >= 0)
            {
                if (pos.x <= pos.z)
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1);
                }
                else
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1);
                }
            }
            else if (pos.x < 0 && pos.z < 0)
            {
                if (pos.x <= pos.z)
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(1, 0, 1, 1);
                }
                else
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 1);
                }
            }
            else
            {
                if (pos.x >= pos.z)
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0, 1);
                }
                else
                {
                    pointCloud.GetComponent<Renderer>().material.color = new Color(0.5f, 0, 1, 1);
                }
            }
            pointCloud.transform.position = pos + origin;
            pointClouds.Add(pointCloud);
        }
    }

    bool isValidPosition(Vector3 pos)
    {
        for (int i = 0; i < pointClouds.Count; i++)
        {
            if (Vector3.Distance(pos, pointClouds[i].transform.position) < minDistance)
                return false;
        }
        return true;
    }
}

class PointCloud
{
    public float x;
    public float y;
    public float z;
    public string colorMin;
    public string colorMax;
    public string audio;

    public PointCloud(float x, float y, float z, string colorMin, string colorMax, string audio)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.colorMin = colorMin;
        this.colorMax = colorMax;
        this.audio = audio;
    }
}