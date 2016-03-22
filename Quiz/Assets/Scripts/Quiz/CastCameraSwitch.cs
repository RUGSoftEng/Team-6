using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;

public class CastCameraSwitch : MonoBehaviour {

    public CastRemoteDisplayManager chromeCast;
    public Camera mainCamera;
    public Camera phoneCamera;

    public void Start()
    {
        chromeCast.RemoteDisplaySessionStartEvent.AddListener(startCasting);
        chromeCast.RemoteDisplaySessionEndEvent.AddListener(stopCasting);
        chromeCast.RemoteDisplayErrorEvent.AddListener(errorCasting);
        phoneCamera.enabled = false;
    }

    public void startCasting(CastRemoteDisplayManager c)
    {
        chromeCast.RemoteDisplayCamera = mainCamera;
        phoneCamera.enabled = true;
    }

    public void stopCasting(CastRemoteDisplayManager c)
    {
        chromeCast.RemoteDisplayCamera = null;
        phoneCamera.enabled = false;
        mainCamera.enabled = true;
    }

    public void errorCasting(CastRemoteDisplayManager c)
    {
        phoneCamera.enabled = false;
        mainCamera.enabled = true;
    }
}
