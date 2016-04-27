using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    public bool quit;
    public GameObject gameController;

    // Checks if the back button is pressed.
    void Update () {
	    if (Input.GetKey(KeyCode.Escape) && Application.platform == RuntimePlatform.Android)
        {
            // if the boolean quit it true than the application has to be closed instead of switched to another screen.
            if (quit)
            {
                Application.Quit();
            } else
            {
                AbstractController ac = gameController.GetComponent<AbstractController>();
                ac.Exit();
            } 
        }
	}
}
