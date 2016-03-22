using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FillInUsername : MonoBehaviour {

    public Text usernameText;

    // Use this for initialization
    void Start() {
        GameObject dataObject = GameObject.FindGameObjectWithTag("ZeeguuData");

        if (dataObject == null) {
            GetComponent<Text>().text = "debug@example.com";
        } else {
            ZeeguuData data = dataObject.GetComponent<ZeeguuData>();
            GetComponent<Text>().text = data.username;
        }
        
	}
}
