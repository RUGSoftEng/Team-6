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
	public Text text;
	
	public void LoadLevel(int index) {
		GameObject zd = GameObject.FindWithTag("ZeeguuData");
        if (zd==null || zd.GetComponent<ZeeguuData>().userBookmarks.Count>minWords) {
            SceneManager.LoadScene(index);
		} else {
			Color c = text.color;
			c.a = 10;
			text.color = c;
			text.GetComponent<Text>().text = "You need at least "+minWords+" words to play that!";
			StartCoroutine(FadeText());
		}
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
    }
}