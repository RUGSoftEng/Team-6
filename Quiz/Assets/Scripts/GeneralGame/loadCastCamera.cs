using UnityEngine;
using Google.Cast.RemoteDisplay;
using System.Collections;
using UnityEngine.UI;

/* this script makes sure that there is allways a camera in the chromecast, so you don't get a boring black screen*/
public class loadCastCamera : MonoBehaviour {

    public Camera chromecastCamera;

	void Start () {
        CastRemoteDisplayManager chromeCast = (CastRemoteDisplayManager)FindObjectOfType(typeof(CastRemoteDisplayManager));
        chromeCast.RemoteDisplayCamera = chromecastCamera;
        chromecastCamera.enabled = true;
    }
}
