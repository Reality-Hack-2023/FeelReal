using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShader : MonoBehaviour
{
    public AudioLoudness audioLoudness;
    public Vector3 scaleMin;
    public Vector3 scaleMax;
    public Material material;
    public Renderer rend;
    public Color colorMax;
    public Color colorMin;
    [Range(0f, 1f)] public float glossiness;
    [Range(0f, 1f)] public float metallic;
    [Range(0, 8)] public float waveFrequency;
    [Range(0, 8)] public float waveAmplitude;

    public float threshold = 0.1f;
    public float loudnessSensitivity = 50;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        colorMin = new Color(1, 0, 0.9423084f, 1);
        colorMax = new Color(0.01568627f, 1, 0.9014204f, 1);
        rend.material = material;
        glossiness = 0.8f;
        metallic = 0.8f;
        waveFrequency = 1;
        waveAmplitude = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = audioLoudness.GetLoudnessFromMic() * loudnessSensitivity;
        if (loudness < threshold) loudness = 0;
        // smooth the loudness value to be very smooth and between scaleMin and scaleMax
        float smoothLoudness = Mathf.Lerp(scaleMin.x, scaleMax.x, loudness);
        // smooth according time delta
        smoothLoudness = Mathf.Lerp(transform.localScale.x, smoothLoudness, Time.deltaTime / 0.5f);
        
        // Debug.Log(smoothLoudness);
        // scale the object
        transform.localScale = new Vector3(smoothLoudness, smoothLoudness, smoothLoudness);
        // change the wave frequency and amplitude based on loudness and smooth it
        waveFrequency = Mathf.Lerp(waveFrequency, loudness * 4, Time.deltaTime / 0.5f);
        waveAmplitude = Mathf.Lerp(waveAmplitude, loudness * 0.5f, Time.deltaTime / 0.5f);
        
       // set the material
        rend.material.SetColor("_ColorMax", colorMax);
        rend.material.SetColor("_ColorMin", colorMin);
        rend.material.SetFloat("_Glossiness", glossiness);
        rend.material.SetFloat("_Metallic", metallic);
        rend.material.SetFloat("_Frequency", waveFrequency);
        rend.material.SetFloat("_Size", waveAmplitude);

    }
    
}
