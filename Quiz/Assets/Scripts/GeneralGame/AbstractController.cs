using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractController : MonoBehaviour {

    public List<WordData> totalWordList;

    /* this method should always be called if a game is quit */
    public void Exit()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        this.GetComponent<LoadNewLevel>().LoadLevel();
    }
}
