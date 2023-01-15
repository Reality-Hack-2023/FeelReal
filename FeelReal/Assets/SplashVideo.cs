using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashVideo : MonoBehaviour
{
    public GameObject loadingBar;
    public Canvas blackScreen;
    public Canvas TextCanvas;
    public string nextScene;
    

    private void Start()
    {
       
    }

    private void Update(){
        // have the loading cube extend width for 3 seconds then load next scene
        if (loadingBar.transform.localScale.x < 300){
            loadingBar.transform.localScale = new Vector3(loadingBar.transform.localScale.x + Time.deltaTime*30, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);
        } else {
            // make image black over one second and then load next scene
            if (blackScreen.GetComponent<Image>().color.a < 1){
                blackScreen.GetComponent<Image>().color = new Color(blackScreen.GetComponent<Image>().color.r, blackScreen.GetComponent<Image>().color.g, blackScreen.GetComponent<Image>().color.b, blackScreen.GetComponent<Image>().color.a + Time.deltaTime);
                TextCanvas.GetComponent<CanvasGroup>().alpha = 1 - blackScreen.GetComponent<Image>().color.a;
            } else {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
