/*
 * This class handles the transition from having everything
 * on your main screen to casting stuff to the chromecast.
 */
using UnityEngine;
using Google.Cast.RemoteDisplay;
using UnityEngine.UI;
using System.Collections;

public class CastCanvasSwitch : MonoBehaviour
{
    public Camera phoneCamera;
    public Camera chromecastCamera;
    public Canvas mainCanvas;
    public Canvas phoneCanvas;
    public Canvas endCanvas;

    private CastRemoteDisplayManager chromeCast;

    public void Start()
    {
        phoneCanvas.enabled = false;
        endCanvas.enabled = false;

        chromeCast = (CastRemoteDisplayManager)FindObjectOfType(typeof(CastRemoteDisplayManager));
        chromeCast.RemoteDisplaySessionStartEvent.AddListener(startCasting);
        chromeCast.RemoteDisplaySessionEndEvent.AddListener(stopCasting);
        chromeCast.RemoteDisplayErrorEvent.AddListener(errorCasting);

        phoneCanvas.enabled = false;
        endCanvas.enabled = false;

        chromeCast.RemoteDisplayCamera = chromecastCamera;

        /* The phone is allready connected to a chromecast*/
        if (chromeCast.IsCasting())
        {
            startCasting(null);
        }
    }

    public void startCasting(CastRemoteDisplayManager c)
    {
        mainCanvas.worldCamera = chromecastCamera;
        phoneCanvas.worldCamera = phoneCamera;
        phoneCanvas.enabled = true;
        GraphicRaycaster gR = mainCanvas.GetComponent<GraphicRaycaster>();
        gR.enabled = false;
    }

    public void stopCasting(CastRemoteDisplayManager c)
    {
        mainCanvas.worldCamera = phoneCamera;
        phoneCanvas.worldCamera = chromecastCamera;
        phoneCanvas.enabled = false;
        GraphicRaycaster gR = mainCanvas.GetComponent<GraphicRaycaster>();
        gR.enabled = true;
    }

    public void errorCasting(CastRemoteDisplayManager c)
    {
        mainCanvas.worldCamera = phoneCamera;
        phoneCanvas.worldCamera = chromecastCamera;
        phoneCanvas.enabled = false;
    }

    public void EndScreen()
    {
        Canvas phone = Instantiate(endCanvas);
        Canvas cast = Instantiate(endCanvas);

        phone.enabled = true;
        mainCanvas.enabled = false;
        phone.worldCamera = phoneCamera;

        cast.enabled = true;
        phoneCanvas.enabled = false;
        cast.worldCamera = chromecastCamera;
        GraphicRaycaster gR = cast.GetComponent<GraphicRaycaster>();
        gR.enabled = false;
    }
}
