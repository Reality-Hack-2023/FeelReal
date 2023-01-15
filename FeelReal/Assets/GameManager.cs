using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static AudioClip micClip;
    private static int sampleWindow = 512;
    public static float micLoudness;
    public static bool isRecording = false;

    public static AudioClip playbackClip;
    public static int clipStart = 0;
    public static bool isPlaying = false;
    public static int timer = 10;

    public static float timeremaining = 1.0f;

    public static int stageState = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }

    public static float GetLoudnessFromMic()
    {
        return GetLoudness(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }
    public static void micToAudioClip()
    {
        string device = Microphone.devices[0];
        micClip = Microphone.Start(device, true, timer, AudioSettings.outputSampleRate);
    }

    public static float GetLoudness(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) startPosition = 0;
        float[] samples = new float[sampleWindow];
        clip.GetData(samples, startPosition);
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        if (isPlaying)
        {
            clipStart += sampleWindow;
        }
        if (clipStart >= clip.samples)
        {
            isPlaying = false;
            clipStart = 0;
            stageState = 3;
        }
        return sum / sampleWindow;
    }
    public static float GetLoudnessFromWAV(int clipPosition, AudioSource source)
    {
        clipStart += sampleWindow;

        float[] samples = new float[sampleWindow];
        source.GetOutputData(samples, 0);
 
        float sum = 0; 
        for (int i = 0; i < sampleWindow; i++)
        {
            Debug.Log(samples[i]);
            sum += Mathf.Abs(samples[i]);
        }
        if (isPlaying)
        {
            clipStart += sampleWindow;
        }
        //Debug.Log(clipStart + " " + source.clip.samples);
        if (clipStart >= source.clip.samples)
        {
            isPlaying = false;
            clipStart = 0;
            stageState = 3;
        }
        return sum / sampleWindow;

    }

}
