using UnityEngine;
using UnityEngine.UI;

public class SwitchColor : MonoBehaviour {

    private Color colorCorrect, colorToDo, colorWrong;

    void Start () {
        Disable();
	}
	
	/*
	 * instantiate the colors of this object.
	 */
	public void SetColors(Color col1, Color col2, Color col3)
    {
        colorCorrect = col1;
        colorToDo = col2;
		colorWrong = col3;
        SetDisabledColor(colorToDo);
    }

	/*
	 * Set the color of this object, after its correspondent word was answered correct.
	 */
    public void Solved()
    {
        SetDisabledColor(colorCorrect);
    }
	
	/*
	 * Set the color of this object, after its correspondent word was answered wrong.
	 */
	public void Wrong()
    {
        SetDisabledColor(colorWrong);
    }

	/*
	 * set the current color of this object.
	 */
    private void SetDisabledColor(Color col)
    {
        var c = GetComponent<Button>().colors;
        c.disabledColor = col;
        GetComponent<Button>().colors = c;
    }

	/*
	 * Make it impossible to interact with this button
	 */
    private void Disable()
    {
        var d = GetComponent<Button>();
        d.interactable = false;
    }
}
