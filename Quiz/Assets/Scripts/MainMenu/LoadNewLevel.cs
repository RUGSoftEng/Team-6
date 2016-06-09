/*
 * Opens up one of the minigames from the main menu.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadNewLevel : MonoBehaviour {
	public int minWords;
    public int maxLength;
	public Text text;
    public Text bottemText;
	
	public void LoadLevel(int index) {
        int cnt = 0;
        GameObject zd = GameObject.FindWithTag("ZeeguuData");
        if (zd==null || zd.GetComponent<ZeeguuData>().userBookmarks.Count>minWords) {
            if (maxLength != 0)
            {
                foreach (Bookmark b in zd.GetComponent<ZeeguuData>().userBookmarks)
                {
                    if (b.word.Length <= maxLength)
                    {
                        cnt++;
                    }
                }
                if (cnt<=minWords)
                {
                    showTooFewWords("You need at least " + minWords + " words with maximal length "+maxLength+" to play that!");
                    return;
                }
            }
            SceneManager.LoadScene(index);
		} else {
            showTooFewWords("You need at least " + minWords + " words to play that!");
		}
    }

    private void showTooFewWords(string message)
    {
        bottemText.enabled = false;
        Color c = text.color;
        c.a = 10;
        text.color = c;
        text.GetComponent<Text>().text = message;
        StartCoroutine(FadeText());
    }
	
	public IEnumerator FadeText()
    {
		Color c = text.color;
		yield return new WaitForSeconds(1F);
		c.a = 0.99F;
		text.color = c;
		for(int i=0;i<=25;i++) {
			yield return new WaitForSeconds(0.04F);
			c = text.color;
			if (c.a>1) {
				return true;
			}
			c.a -= 0.04F;
			text.color = c;
		}
        bottemText.enabled = true;
    }
}