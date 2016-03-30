using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateButton : MonoBehaviour {

	/*
	 * update the word that is displayed on this button
	 */
    public void UpdateText(string text)
    {
        Text textContainer = GetComponent<Transform>().GetComponentInChildren<Text>();
        textContainer.text = text;
    }

	/*
	 * set the color of this button, when it is disabled.
	 */
    public void SetDisabledColor(Color col)
    {
        var c = GetComponent<Button>().colors;
        c.disabledColor = col;
        GetComponent<Button>().colors = c;
    }

    public void Disable(int correct)
    {
        var d = GetComponent<Button>();
		if (correct==1) {
			SetDisabledColor(d.colors.normalColor);
		}
        d.interactable = false;
    }

    public void Enable()
    {
        var d = GetComponent<Button>();
        d.interactable = true;
    }
}
