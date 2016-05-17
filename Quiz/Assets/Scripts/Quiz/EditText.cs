/*
 * This class handles the text that is drawn in the description field of the game.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditText : MonoBehaviour {

    public string beginWord;
    public string endWord;

    public void setText(string description, string word)
    {
        string enhancedDescription = FindMatchingText(description, word);
        this.GetComponent<Text>().text = enhancedDescription;
    }

    private string FindMatchingText(string description, string word)
    {
        char[] descriptionArr = description.ToLower().ToCharArray();
        char[] wordArr = word.ToLower().ToCharArray();

        for (int i = 0; i < descriptionArr.Length; i++)
        {
            if (CheckCurrentIndex(i, descriptionArr, wordArr))
            {
                return addBeginEnd(i, description, word);
            }
        }
        return description.Insert(0, beginWord+word+endWord + "\n");
    }

    private bool CheckCurrentIndex(int i, char[] descriptionArr, char[] wordArr)
    {
        for (int j = 0; j < wordArr.Length; j++)
        {
            Debug.Log(descriptionArr[i]+" != "+ wordArr[j] + (descriptionArr[i] != wordArr[j]));
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

    public void removeText()
    {
        this.GetComponent<Text>().text = "";
    }
}
