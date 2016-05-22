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
	public Color colorCorrect, colorWrong, colorSelected1, colorSelected2, colorNormal1, colorNormal2;
	private int pressed = -1, toDo = 7;
	private List<WordData> words;
	public GameObject end;
	private bool[] locked = new bool[14], heldDown = new bool[14];
	private int difficulty;
	public Text pressText;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
		buttons = GameObject.FindGameObjectsWithTag("memButton").OrderBy( go => go.name ).ToList();
        chromeButtons = GameObject.FindGameObjectsWithTag("chromeButton").OrderBy(go => go.name).ToList();
        LoadData();
		AssignData();
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
		RectTransform rt;
		for(int i=0;i<14;i++) {
			int x,y;
			buttons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
			chromeButtons[i].GetComponent<UpdateButton>().UpdateText(words[i].GetMemoryWord());
			buttons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorNormal1:colorNormal2);
            chromeButtons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorNormal1:colorNormal2);
			buttons[i].GetComponent<UpdateButton>().SetDisabledColor(i%2==0?colorSelected1:colorSelected2);
            chromeButtons[i].GetComponent<UpdateButton>().SetDisabledColor(i%2==0?colorSelected1:colorSelected2);
			rt = buttons[i].GetComponent<RectTransform>();
			rt.offsetMin = new Vector2(0,0);
			rt.offsetMax = new Vector2(0,0);
			do {
				x = 0+rng.Next(0,70);
				y = 6+rng.Next(0,84);
				rt.anchorMin = new Vector2(x/100F,y/100F);
				rt.anchorMax = new Vector2((x+20)/100F,(y+10)/100F);
			} while (overlapping(i));
			rt = chromeButtons[i].GetComponent<RectTransform>();
			rt.anchorMin = new Vector2(x/100F,y/100F);
			rt.anchorMax = new Vector2((x+30)/100F,(y+10)/100F);
			rt.offsetMin = new Vector2(0,0);
			rt.offsetMax = new Vector2(0,0);
		}
		for(int i=0;i<14;i++) {
			rt = buttons[i].GetComponent<RectTransform>();
			int tl = words[i].GetMemoryWord().Length;
			float x = (rt.anchorMax[0]+rt.anchorMin[0])/2;
			float y = (rt.anchorMax[1]+rt.anchorMin[1])/2;
			Vector2 c1 = new Vector2(x-0.02F,y-0.05F);
			Vector2 c2 = new Vector2(x+0.02F,y+0.05F);
			int sz = 4;
			while (!overlapping(i) && (sz)<3*tl && c1[0]>0.02F && c2[0]<0.98F) {
				c1[0]-=0.01F;
				c2[0]+=0.01F;
				sz+=2;
				rt.anchorMin = c1;
				rt.anchorMax = c2;
			}
			if (overlapping(i)) {
				c1[0]+=0.01F;
				c2[0]-=0.01F;
				rt.anchorMin = c1;
				rt.anchorMax = c2;
			}
		}
    }
	
	
	/* checks whether a button is overlapping with any of the other buttons */
	public bool overlapping(int i) {
		RectTransform rt1 = buttons[i].GetComponent<RectTransform>(), rt2;
		Vector2 min1,min2,max1,max2;
		min1 = rt1.anchorMin;
		max1 = rt1.anchorMax;
		for(int j=0;j<14;j++) {
			rt2 = buttons[j].GetComponent<RectTransform>();
			min2 = rt2.anchorMin;
			max2 = rt2.anchorMax;
			if (i==j) {
				
			} else if (((min1[0]<=min2[0] && max1[0]>=min2[0]) || (min2[0]<=min1[0] && max2[0]>=min1[0])) && 
			((min1[1]<=min2[1] && max1[1]>=min2[1]) || (min2[1]<=min1[1] && max2[1]>=min1[1]))) {
				return true;
			}
		}
		return false;
	}
	
	/*decides what happens when any of the 14 buttons is pressed*/
	public void ButtonPressed(int i) {
		heldDown[i]=false;
		if (pressed==i) {
			if (difficulty==0) {
				buttons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorNormal1:colorNormal2);
				chromeButtons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorNormal1:colorNormal2);
				pressed =-1;
			}
		} else if (pressed==-1) {
			buttons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorSelected1:colorSelected2);
            chromeButtons[i].GetComponent<UpdateButton>().SetEnabledColor(i%2==0?colorSelected1:colorSelected2);
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
	
	public void ButtonHold(int i) {
		heldDown[i]=true;
		StartCoroutine(HoldButton(i));
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
			if (difficulty == 0 || (!buttons[i].GetComponent<Button>().interactable && buttons[i].GetComponent<UpdateButton>().GetComponent<Button>().colors.disabledColor!=(i%2==0?colorSelected1:colorSelected2)) || i==pressed) {
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
		buttons[b1].GetComponent<UpdateButton>().SetEnabledColor(b1%2==0?colorNormal1:colorNormal2);
		buttons[b2].GetComponent<UpdateButton>().SetEnabledColor(b2%2==0?colorNormal1:colorNormal2);
		buttons[b1].GetComponent<UpdateButton>().SetDisabledColor(b1%2==0?colorSelected1:colorSelected2);
		buttons[b2].GetComponent<UpdateButton>().SetDisabledColor(b2%2==0?colorSelected1:colorSelected2);
        chromeButtons[b1].GetComponent<UpdateButton>().SetEnabledColor(b1%2==0?colorNormal1:colorNormal2);
        chromeButtons[b2].GetComponent<UpdateButton>().SetEnabledColor(b2%2==0?colorNormal1:colorNormal2);
        chromeButtons[b1].GetComponent<UpdateButton>().SetDisabledColor(b1%2==0?colorSelected1:colorSelected2);
        chromeButtons[b2].GetComponent<UpdateButton>().SetDisabledColor(b2%2==0?colorSelected1:colorSelected2);
        UnlockAllButtons();
		toggleTextVisibility();
    }
	
	IEnumerator HoldButton(int i)
    {
        yield return new WaitForSeconds(1);
		if (heldDown[i]) {
			Color c = pressText.color;
			c.a = 10;
			pressText.color = c;
			pressText.GetComponent<Text>().text = words[i].GetDesc();
			StartCoroutine(FadeText());
		}
    }
	
	public IEnumerator FadeText()
    {
		Color c = pressText.color;
		yield return new WaitForSeconds(2.5F);
		c.a = 0.99F;
		pressText.color = c;
		for(int i=0;i<=25;i++) {
			yield return new WaitForSeconds(0.04F);
			c = pressText.color;
			if (c.a>1) {
				return true;
			}
			c.a -= 0.04F;
			pressText.color = c;
		}
    }

    public override void CreateEndscreen()
    {
        GetComponent<DrawEndCanvas>().EndScreen();
    }
}