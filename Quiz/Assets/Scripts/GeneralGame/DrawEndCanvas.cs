using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawEndCanvas : MonoBehaviour {
    public Camera phoneCamera;
    public Camera chromeCastCamera;
    public Canvas phoneCanvas;
    public Canvas chromeCanvas;
    public Canvas endCanvas;

    public void Start()
    {
        endCanvas.enabled = false;
    }

    public void EndScreen()
    {
        Canvas phone = Instantiate(endCanvas);
        Canvas cast = Instantiate(endCanvas);

        phone.enabled = true;
        phoneCanvas.enabled = false;
        phone.worldCamera = phoneCamera;

        cast.enabled = true;
        chromeCanvas.enabled = false;
        cast.worldCamera = chromeCastCamera;
        GraphicRaycaster gR = cast.GetComponent<GraphicRaycaster>();
        gR.enabled = false;
    }
}
