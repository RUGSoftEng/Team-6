using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    double currentTime;
    bool timing = false;

	// Starts the timing, when a timing is allready running nothing is done.
	public void StartTiming () {
	    if (!timing)
        {
            currentTime = 0;
            timing = true;
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
    public double StopTiming()
    {
        timing = false;
        return currentTime;
    }
}
