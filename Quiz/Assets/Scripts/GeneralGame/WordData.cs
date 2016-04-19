/*
 * A WordData object represents a single word, as used in the game.
 * It is observed by one of the fields of the progress bar.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordData
{
    private string word;
    private string translation;
    private string description;
    private bool solved = false;
    private double seenTime = 0.0;
    private SwitchColor observer;

	public WordData (string word, string translation, string description) {
        this.word = word;
        this.translation = translation;
        this.description = description;	
	}

    public void AddSeenTime(double curSeenTime)
    {
        seenTime = seenTime + curSeenTime;
    }

    public double GetSeenTime()
    {
        return seenTime;
    }
	
	public string GetWord()
    {
        return word;
    }

    public string GetTrans()
    {
        return translation;
    }

    public string GetDesc()
    {
        return description;
    }

    public void setObserver(SwitchColor sc)
    {
        observer = sc;
    }

    public void Solved()
    {
        solved = true;
        observer.Solved();
    }
	
	public void Wrong()
    {
        observer.Wrong();
    }
}
