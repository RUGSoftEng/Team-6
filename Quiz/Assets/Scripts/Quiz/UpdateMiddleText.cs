/*
 * This class handles the words that are displayed in the quiz game.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateMiddleText : MonoBehaviour {

    public Button right;
    public Button left;
    public Color correctCol;
    public Color wrongCol;

	/*
	 * Update the word that is displayed in the middle of the screen and on the answer buttons.
	 */
    public void UpdateText(string ownStr, string trans, string wrongTrans, int correct)
    {
        this.GetComponent<Text>().text = ownStr;
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        if (correct == 0)
        {
            l.UpdateText(trans);
            l.SetDisabledColor(correctCol);
            r.UpdateText(wrongTrans);
            r.SetDisabledColor(wrongCol);
        } else
        {
            l.UpdateText(wrongTrans);
            l.SetDisabledColor(wrongCol);
            r.UpdateText(trans);
            r.SetDisabledColor(correctCol);
        }
    }

	/*
	 * disables the buttons after an answer has been clicked
	 */
    public void DisableButtons()
    {
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        r.Disable();
        l.Disable();
    }

	/*
	 * enables the buttons again so that the answer to the new word can be clicked.
	 */
    public void EnableButtons()
    {
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        r.Enable();
        l.Enable();
    }
}
