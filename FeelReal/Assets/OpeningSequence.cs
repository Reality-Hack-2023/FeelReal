using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSequence : MonoBehaviour
{
    
    public Canvas BlackCanvas;
    public Canvas TextCanvas1;
    public Canvas TextCanvas2;
    public Vector3 Transport1;
    public Vector3 Transport2;

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Sequence();
    }

    // Update is called once per frame
    void Sequence(){
        // fade in and out the canvases one after the other
        StartCoroutine(GameManager.FadeCanvasGroup(BlackCanvas.GetComponent<CanvasGroup>(), 1, 0, 1));
        // wait 1 second
        StartCoroutine(WaitForSeconds(1));
        // fade in and out the canvases one after the other
        StartCoroutine(GameManager.FadeCanvasGroup(BlackCanvas.GetComponent<CanvasGroup>(), 0, 1, 1));
        // wait 1 second
        StartCoroutine(WaitForSeconds(1));
        // fade in and out the canvases one after the other

        StartCoroutine(GameManager.FadeCanvasGroup(BlackCanvas.GetComponent<CanvasGroup>(), 1, 0, 1));
    }

    IEnumerator WaitForSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
    }
}
