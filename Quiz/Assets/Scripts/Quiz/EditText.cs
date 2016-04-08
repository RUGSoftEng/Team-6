/*
 * This class handles the text that is drawn in the description field of the game.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditText : MonoBehaviour {

    public void setText(string newText)
    {
        this.GetComponent<Text>().text = newText;
    }

    public void removeText()
    {
        this.GetComponent<Text>().text = "";
    }
}
