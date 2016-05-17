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
    public string beginWord;
    public string endWord;
    private int correct;

	/*
	 * Update the word that is displayed in the middle of the screen and on the answer buttons.
	 */
    public void UpdateText(string enhancedDescription, string trans, string wrongTrans, int correct)
    {
        this.GetComponent<Text>().text = enhancedDescription;
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
		this.correct = correct;
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
    public void DisableButtons(int correct)
    {
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        if (this.correct==0) {
			r.Disable(correct);
			l.Disable(0);
		} else {
			r.Disable(0);
			l.Disable(correct);
		}
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

    public string FindMatchingText(WordData wd)
    {
        string description = wd.GetDesc();
        string word = wd.GetWord();
        char[] descriptionArr = description.ToLower().ToCharArray();
        char[] wordArr = word.ToLower().ToCharArray();

        for (int i = 0; i < descriptionArr.Length; i++)
        {
            if (CheckCurrentIndex(i, descriptionArr, wordArr))
            {
                return addBeginEnd(i, description, word);
            }
        }
        return description.Insert(0, beginWord + word + endWord + "\n");
    }

    private bool CheckCurrentIndex(int i, char[] descriptionArr, char[] wordArr)
    {
        for (int j = 0; j < wordArr.Length; j++)
        {
            if (descriptionArr[i] != wordArr[j])
            {
                return false;
            }
            i++;
        }
        return true;
    }

    private string addBeginEnd(int pos, string description, string word)
    {
        string enhancedDescription = description.Insert(pos + word.Length, endWord);
        enhancedDescription = enhancedDescription.Insert(pos, beginWord);
        return enhancedDescription;
    }
}
