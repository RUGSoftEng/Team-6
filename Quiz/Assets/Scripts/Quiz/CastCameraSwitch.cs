using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;

public class CastCameraSwitch : MonoBehaviour {

    public CastRemoteDisplayManager chromeCast;
    public Camera phoneCamera;
    public Camera chromecastCamera;
    public Canvas mainCanvas;
    public Canvas phoneCanvas;

    public void Start()
    {
        chromeCast.RemoteDisplaySessionStartEvent.AddListener(startCasting);
        chromeCast.RemoteDisplaySessionEndEvent.AddListener(stopCasting);
        chromeCast.RemoteDisplayErrorEvent.AddListener(errorCasting);
    }

    public void startCasting(CastRemoteDisplayManager c)
    {
        Debug.Log("SuperVreemd");
        mainCanvas.worldCamera = chromecastCamera;
        phoneCanvas.worldCamera = phoneCamera;
    }

    public void stopCasting(CastRemoteDisplayManager c)
    {
        mainCanvas.worldCamera = phoneCamera;
        phoneCanvas.worldCamera = null;
    }

    public void errorCasting(CastRemoteDisplayManager c)
    {
        mainCanvas.worldCamera = phoneCamera;
        phoneCanvas.worldCamera = null;
    }
}
