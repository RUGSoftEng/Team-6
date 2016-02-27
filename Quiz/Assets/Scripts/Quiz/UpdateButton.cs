using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateButton : MonoBehaviour {

    public void UpdateText(string text)
    {
        Text textContainer = GetComponent<Transform>().GetComponentInChildren<Text>();
        textContainer.text = text;
    }

    public void SetPressedColor(Color col)
    {
        var c = GetComponent<Button>().colors;
        c.pressedColor = col;
        GetComponent<Button>().colors = c;
    }
}
