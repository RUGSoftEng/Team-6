using UnityEngine;
using System.Collections;

public class AbstractController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /* this method should always be called if the quiz game is exitted */
    protected void Exit()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        this.GetComponent<LoadNewLevel>().LoadLevel();
    }
}
