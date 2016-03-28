using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordData
{
    private string word;
    private string translation;
    private string description;
    private bool solved = false;
    private SwitchColor observer;

	public WordData (string word, string translation, string description) {
        this.word = word;
        this.translation = translation;
        this.description = description;	
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
