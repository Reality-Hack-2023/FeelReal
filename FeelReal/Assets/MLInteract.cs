using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class MLInteract : MonoBehaviour
{
    public string emotion;

    void Start() { }

    public void getMood()
    {
        getAPI("http://127.0.0.1:8000/ser");
        playSounds();
    }

    private void getAPI(string api)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        emotion = jsonResponse;
    }

    private void playSounds()
    {
        AudioSource audio = GameObject.FindWithTag("as").GetComponent<AudioSource>();
        switch (emotion)
        {
            case "\"ANGRY\"":
                audio.clip = Resources.Load("Angry") as AudioClip; ;
                break;
            case "\"CALM\"":
                audio.clip = Resources.Load("Calm") as AudioClip; ;
                break;
            case "\"DISGUST\"":
                audio.clip = Resources.Load("Disgust") as AudioClip; ;
                break;
            case "\"FEARFUL\"":
                audio.clip = Resources.Load("Fearful") as AudioClip; ;
                break;
            case "\"HAPPY\"":
                audio.clip = Resources.Load("Joyful") as AudioClip; ;
                break;
            case "\"NEUTRAL\"":
                audio.clip = Resources.Load("Neutral") as AudioClip; ;
                break;
            case "\"SAD\"":
                audio.clip = Resources.Load("Sad") as AudioClip; ;
                break;
            case "\"SURPRISED\"":
                audio.clip = Resources.Load("Surprise") as AudioClip; ;
                break;
        }
        Debug.Log(emotion);
        // get gameobject with tag of Heart
        GameObject heart = GameObject.FindWithTag("Heart");
        // get the script attached to the heart
        ChangeShader heartScript = heart.GetComponent<ChangeShader>();
        // call the function on the script
        heartScript.ChangeAudio(SetSampleRate(audio.clip, AudioSettings.outputSampleRate));
        GameManager.clipStart = 0;
        audio.Play();
    }

    public static AudioClip SetSampleRate(AudioClip clip, int frequency)
    {
        if (clip.frequency == frequency) return clip;
        if (clip.channels != 1 && clip.channels != 2) return clip;

        var samples = new float[clip.samples * clip.channels];

        clip.GetData(samples, 0);

        var samplesNewLength = (int)(frequency * clip.length) * clip.channels;
        var clipNew = AudioClip.Create(clip.name + "_" + frequency, samplesNewLength, clip.channels, frequency, false);

        var channelsOriginal = new List<float[]>();
        var channelsNew = new List<float[]>();

        if (clip.channels == 1)
        {
            channelsOriginal.Add(samples);
            channelsNew.Add(new float[(int)(frequency * clip.length)]);
        }
        else
        {
            channelsOriginal.Add(new float[clip.samples]);
            channelsOriginal.Add(new float[clip.samples]);

            channelsNew.Add(new float[(int)(frequency * clip.length)]);
            channelsNew.Add(new float[(int)(frequency * clip.length)]);

            for (var i = 0; i < samples.Length; i++)
            {
                channelsOriginal[i % 2][i / 2] = samples[i];
            }
        }

        for (var c = 0; c < clip.channels; c++)
        {
            var index = 0;
            var sum = 0f;
            var count = 0;
            var channelSamples = channelsOriginal[c];

            for (var i = 0; i < channelSamples.Length; i++)
            {
                var index_ = (int)((float)i / channelSamples.Length * channelsNew[c].Length);

                if (index_ == index)
                {
                    sum += channelSamples[i];
                    count++;
                }
                else
                {
                    channelsNew[c][index] = sum / count;
                    index = index_;
                    sum = channelSamples[i];
                    count = 1;
                }
            }
        }

        float[] samplesNew;

        if (clip.channels == 1)
        {
            samplesNew = channelsNew[0];
        }
        else
        {
            samplesNew = new float[channelsNew[0].Length + channelsNew[1].Length];

            for (var i = 0; i < samplesNew.Length; i++)
            {
                samplesNew[i] = channelsNew[i % 2][i / 2];
            }
        }

        clipNew.SetData(samplesNew, 0);

        return clipNew;
    }

}
