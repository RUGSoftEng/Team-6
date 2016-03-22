using UnityEngine;
using System.Collections;

public class WordData
{
    private string word;
    private string translation;
    private string description;

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
}
