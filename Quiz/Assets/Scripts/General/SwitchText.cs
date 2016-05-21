using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchText : MonoBehaviour {

    public List<string> strList;
    
	void Start () {
        this.GetComponent<Text>().text = strList[Random.Range(0, strList.Count)]; ;
	}

}
