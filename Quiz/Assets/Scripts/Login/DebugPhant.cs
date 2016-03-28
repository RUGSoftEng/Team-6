using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugPhant : MonoBehaviour {

    public GameObject data;

    public void DebugLogin() {
        ZeeguuData zdata = data.GetComponent<ZeeguuData>();
        //zdata.Login("deliberatelywrong ;P", "Notgivingyoumypassword <3", "");
    }
}
