using UnityEngine;
using System.Collections;

public class LoadNewLevel : MonoBehaviour {
    public int levelIndex;

	public void LoadLevel() {
        Application.LoadLevel(levelIndex);
    }
}
