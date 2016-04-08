/*
 * Opens up one of the minigames from the main menu.
 */
using UnityEngine;
using System.Collections;

public class LoadNewLevel : MonoBehaviour {
    public int levelIndex;

	public void LoadLevel() {
        Application.LoadLevel(levelIndex);
    }
}
