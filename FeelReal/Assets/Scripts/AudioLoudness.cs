using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudness : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip micClip;
    // Start is called before the first frame update
    void Start()
    {
        micToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetLoudnessFromMic(){
        return GetLoudness(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }
    public void micToAudioClip(){
        string device = Microphone.devices[0];
        micClip = Microphone.Start(device, true, 300, AudioSettings.outputSampleRate);
    }

    public float GetLoudness(int clipPosition, AudioClip clip){
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) startPosition = 0;
        float[] samples = new float[sampleWindow];
        clip.GetData(samples, startPosition);
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++){
            sum += Mathf.Abs(samples[i]);
        }
        return sum / sampleWindow;

    }
}
