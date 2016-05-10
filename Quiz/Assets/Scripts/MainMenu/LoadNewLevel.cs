/*
 * Opens up one of the minigames from the main menu.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadNewLevel : MonoBehaviour {
	public int minWords;
	
	public void LoadLevel(int index) {
		GameObject zd = GameObject.FindWithTag("ZeeguuData");
        if (zd.GetComponent<ZeeguuData>().userBookmarks.Count>minWords) {
			Application.LoadLevel(index);
		}
    }
}