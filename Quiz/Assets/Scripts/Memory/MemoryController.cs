/*
 * This is the main class of
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class MemoryController : AbstractController {
	
	private List<GameObject> buttons, chromeButtons;
    private Timer timer;
	public Color colorCorrect, colorWrong, colorSelected, colorNormal;
	private int pressed = -1, toDo = 7;
	private List<WordData> words;
	public GameObject end;
	private bool[] locked = new bool[14];
	private int difficulty;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
		buttons = GameObject.FindGameObjectsWithTag("memButton").OrderBy( go => go.name ).ToList();
        chromeButtons = GameObject.FindGameObjectsWithTag("chromeButton").OrderBy(go => go.name).ToList();
        LoadData();
		AssignData();
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
		/*int n = words.Count;
		while (n>1) {
			n--;
			int k = rng.Next(0,n-1);
			WordData v = words[k];
			words[k] = words[n];
			words[n] = v;
		}*/
		System.Random rng = new System.Random();
		int x,y;
		RectTransform rt;
		for(int i=0;i<14;i++) {
			buttons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
			chromeButtons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
            chromeButtons[i].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
			rt = buttons[i].GetComponent<RectTransform>();
			x = rng.Next(0,Screen.width);
			y = rng.Next(0,Screen.height);
			rt.anchorMin = new Vector2(x,y);
			rt.anchorMax = new Vector2(x+Screen.width/10,y+Screen.height/10);
			rt = chromeButtons[i].GetComponent<RectTransform>();
			x = rng.Next(0,Screen.width);
			y = rng.Next(0,Screen.height);
			rt.anchorMin = new Vector2(x,y);
			rt.anchorMax = new Vector2(x+Screen.width/10,y+Screen.height/10);
		}
    }
	
	public void ButtonPressed(int i) {
		if (pressed==i) {
			if (difficulty==0) {
				buttons[i].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
				chromeButtons[i].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
				pressed =-1;
			}
		} else if (pressed==-1) {
			buttons[i].GetComponent<UpdateButton>().SetEnabledColor(colorSelected);
            chromeButtons[i].GetComponent<UpdateButton>().SetEnabledColor(colorSelected);
            pressed = i;
		} else if (words[i].GetWord()==words[pressed].GetWord()) {
			buttons[i].GetComponent<Button>().interactable = false;
			buttons[pressed].GetComponent<Button>().interactable = false;
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);
			buttons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);

            chromeButtons[i].GetComponent<Button>().interactable = false;
            chromeButtons[pressed].GetComponent<Button>().interactable = false;
            chromeButtons[i].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);
            chromeButtons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorCorrect);

            pressed =-1;
			toDo--;
		} else {
			LockAllButtons();
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
            chromeButtons[i].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
            buttons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
            chromeButtons[pressed].GetComponent<UpdateButton>().SetDisabledColor(colorWrong);
			StartCoroutine(WaitButtons(pressed,i));
			pressed=-1;
		}
		toggleTextVisibility();
		if (toDo==0) {
			CreateEndscreen();
			return;
		}
	}
	
	public void LockAllButtons() {
		for (int i=0;i<14;i++) {
			locked[i] = buttons[i].GetComponent<Button>().interactable;
			buttons[i].GetComponent<Button>().interactable = false;
            chromeButtons[i].GetComponent<Button>().interactable = false;
		}
    }
	
	public void UnlockAllButtons() {
		for (int i=0;i<14;i++) {
			buttons[i].GetComponent<Button>().interactable = locked[i];
            chromeButtons[i].GetComponent<Button>().interactable = locked[i];
		}
    }
	
	public void switchDifficulty() {
		if (difficulty==0) {
			difficulty = 1;
			GameObject.FindGameObjectsWithTag("diffSwitch")[0].GetComponent<Transform>().GetComponentInChildren<Text>().text = "Difficulty: Hard";
		} else {
			difficulty = 0;
			GameObject.FindGameObjectsWithTag("diffSwitch")[0].GetComponent<Transform>().GetComponentInChildren<Text>().text = "Difficulty: Easy";
		}
		toggleTextVisibility();
	}
	
	private void toggleTextVisibility() {
		for(int i=0;i<14;i++) {
			if (difficulty == 0 || (!buttons[i].GetComponent<Button>().interactable && buttons[i].GetComponent<UpdateButton>().GetComponent<Button>().colors.disabledColor!=colorSelected) || i==pressed) {
				buttons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
				chromeButtons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
			} else {
				buttons[i].GetComponent<UpdateButton>().UpdateText("");
				chromeButtons[i].GetComponent<UpdateButton>().UpdateText("");
			}
		}
	}
	
	IEnumerator WaitButtons(int b1, int b2)
    {
        yield return new WaitForSeconds(2.5F);
		buttons[b1].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
		buttons[b2].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
		buttons[b1].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
		buttons[b2].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
        chromeButtons[b1].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
        chromeButtons[b2].GetComponent<UpdateButton>().SetEnabledColor(colorNormal);
        chromeButtons[b1].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
        chromeButtons[b2].GetComponent<UpdateButton>().SetDisabledColor(colorSelected);
        UnlockAllButtons();
		toggleTextVisibility();
    }

    public override void CreateEndscreen()
    {
        GetComponent<DrawEndCanvas>().EndScreen();
    }
}