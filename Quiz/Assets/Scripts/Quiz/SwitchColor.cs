using UnityEngine;
using UnityEngine.UI;

public class SwitchColor : MonoBehaviour {

    private Color colorCorrect, colorToDo;

    void Start () {
        Disable();
	}
	
	public void SetColors(Color col1, Color col2)
    {
        colorCorrect = col1;
        colorToDo = col2;
        SetDisabledColor(colorToDo);
    }

    public void Solved()
    {
        SetDisabledColor(colorCorrect);
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
}
