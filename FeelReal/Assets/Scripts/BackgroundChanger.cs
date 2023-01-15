using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    public Color groundStartColor = new Color(31 / 255f, 29 / 255f, 29 / 255f, 1.0f);
    public Color groundEndColor;

    public Color skyStartColor = new Color(78 / 255f, 14 / 255f, 87 / 255f, 1.0f);
    public Color skyEndColor;

    public float atmosphereStart = 0.28f;
    public float atmosphereEnd;

    public float changeDuration = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetColor("_GroundColor",groundStartColor);
        RenderSettings.skybox.SetColor("_SkyTint", skyStartColor);
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", atmosphereStart);
    }

    // Update is called once per frame
    void Update()
    {
        //float t = Mathf.PingPong(Time.time, changeDuration) / changeDuration;
        //float t = Mathf.Repeat(Time.time, changeDuration) / changeDuration;
        if (changeDuration > 0.0f)
        {
            float t = Time.time / changeDuration;
            Mathf.Clamp(t, 0.0f, 1.0f);

            ChangeToEmotions();
        }

    }

    private void ChangeToEmotions()
    {
        RenderSettings.skybox.SetColor("_GroundColor", Color.Lerp(groundStartColor, groundEndColor, changeDuration));
        RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(skyStartColor, skyEndColor, changeDuration));
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", Mathf.Lerp(atmosphereStart, atmosphereEnd, changeDuration));
    }

    public void SetNewColors(Color groundColor, Color skyColor, float atmosphere, float duration)
    {
        groundEndColor = groundColor;
        skyEndColor = skyColor;
        atmosphereEnd = atmosphere;
        changeDuration = duration;
    }

    public void ChangeBack()
    {
        RenderSettings.skybox.SetColor("_GroundColor", Color.Lerp(groundEndColor, groundStartColor, changeDuration));
        RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(skyEndColor, skyStartColor, changeDuration));
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", Mathf.Lerp(atmosphereEnd, atmosphereStart, changeDuration));
    }
}
