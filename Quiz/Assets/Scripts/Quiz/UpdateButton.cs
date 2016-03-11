using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateButton : MonoBehaviour {

    public void UpdateText(string text)
    {
        Text textContainer = GetComponent<Transform>().GetComponentInChildren<Text>();
        textContainer.text = text;
    }

    public void SetDisabledColor(Color col)
    {
        var c = GetComponent<Button>().colors;
        c.disabledColor = col;
        GetComponent<Button>().colors = c;
    }

    public void Disable()
    {
        var d = GetComponent<Button>();
        d.interactable = false;
    }

    public void Enable()
    {
        var d = GetComponent<Button>();
        d.interactable = true;
    }
}
