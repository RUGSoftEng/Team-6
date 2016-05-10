using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;
using UnityEngine.UI;

/* this script makes sure that there is allways a camera in the chromecast, so you don't get a boring black screen*/
public class loadCastCamera : MonoBehaviour {

    public Camera phoneCamera;
    public Camera chromecastCamera;
    public Canvas phoneCanvas;
    public Canvas chromeCanvas;
    public Canvas endCanvas;

	void Start () {
        CastRemoteDisplayManager chromeCast = (CastRemoteDisplayManager)FindObjectOfType(typeof(CastRemoteDisplayManager));
        chromeCast.RemoteDisplayCamera = chromecastCamera;
        chromecastCamera.enabled = true;
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
        cast.worldCamera = chromecastCamera;
        GraphicRaycaster gR = cast.GetComponent<GraphicRaycaster>();
        gR.enabled = false;
    }
}
