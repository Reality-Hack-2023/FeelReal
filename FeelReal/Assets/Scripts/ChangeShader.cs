using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShader : MonoBehaviour
{
    public AudioLoudness audioLoudness;
    public AudioSource source;
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
        // transform is 0
        transform.localScale = new Vector3(0, 0, 0);
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
        Debug.Log("State: " + GameManager.stageState);
        if (GameManager.stageState == 1)
        {
            ExpandOverTime(0.7f);
        }
        else if (GameManager.stageState == 2)
        {
            bool idle = GameManager.isRecording;
            float loudness = 0;
            if (GameManager.isPlaying)
            {
                idle = false;
                loudness = GameManager.GetLoudnessFromWAV(GameManager.clipStart, source) * 10f;
                Debug.Log(loudness);
            }
            else
            {
                loudness = (idle) ? GameManager.GetLoudnessFromMic() * loudnessSensitivity : 0.2f;
            }
            if (loudness < threshold)
            {
                loudness = 0.2f;
                // time 2 seconds
            }
            // smooth the loudness value to be very smooth and between scaleMin and scaleMax
            float smoothLoudness = Mathf.Lerp(scaleMin.x, scaleMax.x, loudness);
            // smooth according time delta
            smoothLoudness = Mathf.Lerp(transform.localScale.x, smoothLoudness, Time.deltaTime / 2f);
            // scale the object
            transform.localScale = new Vector3(smoothLoudness, smoothLoudness, smoothLoudness);
            // change the wave frequency and amplitude based on loudness and smooth it
            waveFrequency = idle ? Mathf.Lerp(waveFrequency, loudness * 2, Time.deltaTime / 10f) : 1f;
            waveAmplitude = idle ? Mathf.Lerp(waveAmplitude, loudness * 0.5f, Time.deltaTime / 2f) : 1.2f;
            colorMax = !idle ? new Color(1, 1, 1, 1) : new Color(1, 0, 0.9423084f, 1);
            colorMin = !idle ? new Color(1, 1, 1, 1) : new Color(0.01568627f, 1, 0.9014204f, 1);
            // set the material
            //rend.material.SetColor("_ColorMax", colorMax);
            //rend.material.SetColor("_ColorMin", colorMin);
            rend.material.SetFloat("_Glossiness", glossiness);
            rend.material.SetFloat("_Metallic", metallic);
            rend.material.SetFloat("_Frequency", waveFrequency);
            rend.material.SetFloat("_Size", waveAmplitude);
        }
        else
        {
            GameManager.isPlaying = false;
        }

    }

    void ExpandOverTime(float time)
    {
        StartCoroutine(Expand(time));
        GameManager.stageState = 2;
    }

    IEnumerator Expand(float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), scaleMin, t / time);
            rend.material.SetFloat("_Frequency", 1);
            rend.material.SetFloat("_Size", 1 - t / time);
            yield return null;
        }
    }

    public void ChangeAudio(AudioClip clip)
    {
        // change audio source of this object
        source.clip = clip;
    }

    public void ChangeColor(Color min, Color max)
    {
        StartCoroutine(ColorChangeOverTime(1, min, max));
        
    }

    IEnumerator ColorChangeOverTime(float time, Color min, Color max)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            Color colorMinGrad = Color.Lerp(colorMin, min, t);
            Color colorMaxGrad = Color.Lerp(colorMax, max, t);
            rend.material.SetColor("_ColorMin", colorMinGrad);
            rend.material.SetColor("_ColorMax", colorMaxGrad);
            yield return null;
        }
    }



}
