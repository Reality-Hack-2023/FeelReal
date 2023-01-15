using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBallSound : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    private bool played = false;
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.stageState >= 3 && !played)
        {
            audioSource.Play();
            played = true;
        }
    }
}
