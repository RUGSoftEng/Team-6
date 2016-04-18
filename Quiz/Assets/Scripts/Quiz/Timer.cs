using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    double currentTime;
    bool timing = false;

	// Starts the timing, when a timing is allready running nothing is done.
	void StartTiming () {
	    if (!timing)
        {
            currentTime = 0;
        }
	}
	
	// Update is called once per frame, it updates the time that is timed untill now.
	void Update () {
        if (timing)
        {
            currentTime = currentTime + Time.deltaTime;
        }
	}

    //Stops the timer and returns the amount of time that is timed.
    double StopTiming()
    {
        timing = false;
        return currentTime;
    }
}
