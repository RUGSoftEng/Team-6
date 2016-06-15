using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchText : MonoBehaviour {

    private int i = -1;
    public List<string> strList;
    public Text chromeCastText;
    
	void Start () {
        SetRandom();
	}

    public void NotRandom(int i)
    {
        Debug.Log("set string to: " + i);
        this.i = i;
        this.GetComponent<Text>().text = strList[i];
        if (chromeCastText != null)
        {
            chromeCastText.GetComponent<Text>().text = strList[i];
        }
    }

    public void SetRandom ()
    {
        Debug.Log("set string to random");
        int x;
        do 
        {
            x = Random.Range(0, strList.Count);
        } while (x == this.i);
        NotRandom (x);
    }
}
