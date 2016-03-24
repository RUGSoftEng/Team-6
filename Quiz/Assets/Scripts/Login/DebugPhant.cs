using UnityEngine;
using System.Collections;

public class DebugPhant : MonoBehaviour {

    public GameObject data;

    public void DebugLogin() {
        ZeeguuData zdata = data.GetComponent<ZeeguuData>();

        zdata.username = "debug@example.com";
        zdata.userLearnedLanguage = "de";
        zdata.sessionID = "debug";

        Application.LoadLevel(1);

    }
}
