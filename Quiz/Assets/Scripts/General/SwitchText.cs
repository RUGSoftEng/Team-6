using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchText : MonoBehaviour {

    private int i;
    public List<string> strList;
    
	void Start () {
        Debug.Log("set string to random");
        this.GetComponent<Text>().text = strList[Random.Range(0, strList.Count)];
	}

    public void NotRandom(int i)
    {
        Debug.Log("set string to: " + i);
        this.i = i;
        this.GetComponent<Text>().text = strList[i];
    }

    void Update()
    {
        this.GetComponent<Text>().text = strList[i];
    }
}
