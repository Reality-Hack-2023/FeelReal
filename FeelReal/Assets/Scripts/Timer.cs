using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float startTime;
    private float totalTime = 10f; // Set the total time to 10 seconds
    private bool finnished = false;

    public Material material;
    public Renderer rend;
    public GameObject timerVisual;

    void Start()
    {
        startTime = -1;

        rend = timerVisual.GetComponent<Renderer>();
        rend.material = material;
        Debug.Log(rend.material);
    }

    void Update()
    {
        if (GameManager.isRecording)
        {
            if (startTime < 0) startTime = Time.time;

            if (finnished)
            {
                startTime = -1;
                return;
            }

            float t = totalTime - (Time.time - startTime);
            GameManager.timeremaining = t / 10f;
            Debug.Log(t);

            //link variable
            rend.material.SetFloat("_CompletionSlider", GameManager.timeremaining);

            Debug.Log(rend.material.GetFloat("_CompletionSlider"));

            if (t <= 0f)
            {
                finnished = true;
                timerText.text = "0.00";
                return;
            }

            string seconds = (t % 60).ToString("f2");

            timerText.text = seconds;
        }
    }
}