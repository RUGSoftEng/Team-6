/*
 * Opens up one of the minigames from the main menu.
 */
using UnityEngine;

public class LoadNewLevel : MonoBehaviour {
	public void LoadLevel(int index) {
        Application.LoadLevel(index);
    }
}
