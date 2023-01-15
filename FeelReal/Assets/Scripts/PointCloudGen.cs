using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudGen : MonoBehaviour
{

    public GameObject particleCloudPrefab;
    public GameObject audioLoudnessPrefab;
    public int numPoints;
    public float minDistance = 3;
    public float length = 100;
    public float zRange = 25;
    public float amplitude = 1;
    public float frequency = 1;
    public float stretchFactor = 10;
    public Material[] materials;  // assign different colors in the Unity editor

    private List<GameObject> pointClouds = new List<GameObject>();

    void Start()
    {
        materials = new Material[9];
        for (int i = 0; i < 9; i++)
        {
            materials[i] = new Material(Shader.Find("Unlit/Color"));
            materials[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        genPointCloud(GameObject.Find("Point").transform.position);
    }

    float offset = 0;

    void Update()
    {
        float deltaTime = Time.deltaTime;
        float animationSpeed = 2.0f;
        float bottomAmplitude = -amplitude;
        float topAmplitude = amplitude;
        offset += Time.deltaTime * animationSpeed;
        foreach (GameObject pointCloud in pointClouds)
        {
            Vector3 currentPosition = pointCloud.transform.position;
            float animationValue = amplitude * Mathf.Sin((pointCloud.transform.position.x + offset) * frequency);
            Vector3 newPosition = new Vector3(currentPosition.x, animationValue, currentPosition.z);
            pointCloud.transform.position = Vector3.Lerp(currentPosition, newPosition, deltaTime);
        }
    }


    void genPointCloud(Vector3 origin)
    {
        for (int j = 0; j < zRange * 4; j += 4)
        {
            float z = j;
            for (int i = 0; i < numPoints; i++)
            {
                float newX = i * (length / numPoints) - length / 2;
                float x = i * (length / numPoints) - length / 2;
                float y = amplitude * Mathf.Cos(x * frequency * stretchFactor);
                Vector3 pos = new Vector3(x, y, z);
                int runs = 0;
                while (!isValidPosition(pos, origin))
                {
                    if (++runs >= 100) break;
                    newX = i * (length / numPoints) - length / 2;
                    float newY = amplitude * Mathf.Sin(newX * frequency * stretchFactor);
                    float newZ = Random.Range(-zRange / 2, zRange / 2);
                    Vector3 newPos = new Vector3(newX, newY, newZ);
                    pos = newPos;
                }
                if (runs >= 100)
                {
                    Debug.Log("Only Generated " + i + " nodes");
                    break;
                }
                GameObject pointCloud = Instantiate(particleCloudPrefab, pos + origin, Quaternion.identity);
                pointCloud.GetComponent<Renderer>().material = materials[(int)((newX + length / 2) / (length / 9)) % 9];
                pointClouds.Add(pointCloud);
            }
        }

    }

    bool isValidPosition(Vector3 pos, Vector3 origin)
    {
        for (int i = 0; i < pointClouds.Count; i++)
        {
            if (Vector3.Distance(pos, pointClouds[i].transform.position - origin) < minDistance) return false;
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