﻿/*
 * This is the main class of
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class MemoryController : AbstractController {
	
	private List<GameObject> buttons;
    private Timer timer;
	public Color colorCorrect, colorWrong, colorSelected;
	private int pressed = -1, toDo = 7;
	private List<WordData> words;
	public GameObject end;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
		buttons = GameObject.FindGameObjectsWithTag("memButton").OrderBy( go => go.name ).ToList();
        LoadData();
		AssignData();
		for (int i=0;i<14;i++) {
			SwitchColor sC = buttons[i].GetComponent<SwitchColor>();
			sC.SetColors(colorCorrect, colorSelected, colorWrong);
			totalWordList[i].setObserver(sC);
		}
        UpdateGame();
	}
	
	// Update is called once per frame
	void UpdateGame () {
	
	}
	
	void AssignData() {
		words = new List<WordData>();
		for(int i=0;i<7;i++) {
			words.Add(new WordData(totalWordList[i]));
			words[2*i].SetMemory(false);
			words.Add(totalWordList[i]);
			words[2*i+1].SetMemory(true);
		}
		System.Random rng = new System.Random();
		int n = words.Count;
		while (n>1) {
			n--;
			int k = rng.Next(0,n-1);
			WordData v = words[k];
			words[k] = words[n];
			words[n] = v;
		}
		for(int i=0;i<14;i++) {
			buttons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
		}
	}
	
	public void ButtonPressed(int i) {
		buttons[i].GetComponent<Button>().interactable = false;
		if (pressed==-1) {
			pressed = i;
		} else if (words[i].GetWord()==words[pressed].GetWord()) {
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);
			buttons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);
			pressed=-1;
			toDo--;
		} else {
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
			buttons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
			StartCoroutine(WaitButtons(pressed,i));
			pressed=-1;
		}
		if (toDo==0) {
			CreateEndscreen(end);
            StartCoroutine(WaitFinished());
			return;
		}
	}
	
	IEnumerator WaitButtons(int b1, int b2)
    {
        yield return new WaitForSeconds(1);
		buttons[b1].GetComponent<Button>().interactable = true;
		buttons[b2].GetComponent<Button>().interactable = true;
		buttons[b1].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
		buttons[b2].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
    }
}