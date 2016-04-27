/*
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
	public Color colorCorrect, colorToDo, colorWrong;
	private int pressed = -1;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
		buttons = GameObject.FindGameObjectsWithTag("memButton").OrderBy( go => go.name ).ToList();
        LoadData();
		for (int i=0;i<14;i++) {
			SwitchColor sC = buttons[i].GetComponent<SwitchColor>();
			sC.SetColors(colorCorrect, colorToDo, colorWrong);
			totalWordList[i].setObserver(sC);
		}
        UpdateGame();
	}
	
	/*
	 * The loadData method selects the words to be used in the game.
	 */
	private void LoadData()
    {
        totalWordList = new List<WordData>();
        GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");
        if (zeeguuList.Length<1)
        {
            Debug.Log("No zeeguuData Available, using hardcoded Set");
            totalWordList.Add(new WordData("Lion", "Leeuw", "A Lion Roars"));
            totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts to Mary"));
            totalWordList.Add(new WordData("Surf", "Surfen", "Harold loves to surf"));
            totalWordList.Add(new WordData("Moan", "Zeuren", "Jimmy moans a lot"));
        } else
        {
            List<Bookmark> localBookmarkList = new List<Bookmark>(zeeguuList[0].GetComponent<ZeeguuData>().userBookmarks);
            for (int i=0; i<14; i++)
            {
                if (localBookmarkList.Count == 0)
                {
                    break;
                }
                int randomIndex = Random.Range(0, localBookmarkList.Count);
                totalWordList.Add(new WordData(localBookmarkList[randomIndex].word, localBookmarkList[randomIndex].translation, localBookmarkList[randomIndex].context));
                localBookmarkList.RemoveAt(randomIndex);
            }
        }
    }
	
	// Update is called once per frame
	void UpdateGame () {
	
	}
	
	public void ButtonPressed(int i) {
		buttons[i].GetComponent<Button>().interactable = false;
		Debug.Log(buttons.Count+"");
		if (pressed==-1) {
			pressed = i;
		} else {
			StartCoroutine(WaitButtons(pressed,i));
			pressed=-1;
		}
	}
	
	IEnumerator WaitButtons(int b1, int b2)
    {
        yield return new WaitForSeconds(1);
		buttons[b1].GetComponent<Button>().interactable = true;
		buttons[b2].GetComponent<Button>().interactable = true;
    }
}