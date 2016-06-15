using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawEndCanvas : MonoBehaviour {
    public Camera phoneCamera;
    public Camera chromeCastCamera;
    public Canvas phoneCanvas;
    public Canvas chromeCanvas;
    public Canvas phoneEndCanvas;
    public Canvas chromeEndCanvas;

    public void Start()
    {
        phoneEndCanvas.enabled = false;
        chromeEndCanvas.enabled = false;
    }

    public void EndScreen()
    {
        phoneEndCanvas.enabled = true;
        phoneCanvas.enabled = false;
        phoneEndCanvas.worldCamera = phoneCamera;

        chromeEndCanvas.enabled = true;
        chromeCanvas.enabled = false;
        chromeEndCanvas.worldCamera = chromeCastCamera;
        GraphicRaycaster gR = chromeEndCanvas.GetComponent<GraphicRaycaster>();
        gR.enabled = false;
    }
}
