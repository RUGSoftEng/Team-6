/*
 * This class is the main controller of the quiz game. It decides what words are displayed.
 *
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Google.Cast.RemoteDisplay;

public class QuizController : AbstractController {

    public Text middleText;
    public float correctWaitTime;
    public float wrongWaitTime;
    public List<WordData> toDoList;
    private WordData currentWord, nextWord;
    private int correct; //0 is left   1 is right
	public int numberOfClicks;
    private bool fastClickFixer = true; //To fix the delay of disabeling buttons 
    private Timer timer;
    private string descWithWord;
    public GameObject end;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
        numberOfClicks = 0;
        LoadData();
        toDoList = new List<WordData>(totalWordList);
        InstantiateGame();
        UpdateGame();
    }


    /* this allready makes sure everything is ready to go when the game has to continue after the disable.*/
    private void InstantiateGame()
    {
        if (toDoList.Count < 1)
        {
            return;
        }
        nextWord = toDoList[0];
        descWithWord = middleText.GetComponent<UpdateMiddleText>().FindMatchingText(nextWord);
    }

    /*
     * The UpdateGame method makes sure everything in the game is correct at any point in time.
     * It sets the right words for the buttons and selects new ones if needed.
     */
    private void UpdateGame()
    {
        if (toDoList.Count < 1)
        {
			CreateEndscreen();
			return;
        }

        currentWord = nextWord;
        string wrongTrans;
        do
        {
            wrongTrans = SelectRandomWord(totalWordList).GetTrans();
        } while (currentWord.GetTrans().Equals(wrongTrans));
        correct = UnityEngine.Random.Range(0, 2);
        middleText.GetComponent<UpdateMiddleText>().UpdateText(descWithWord, currentWord.GetTrans(), wrongTrans, correct);
        timer.StartTiming();
    }
	
	/*
	 * returns a random word from a wordlist.
	 */
    private WordData SelectRandomWord(List<WordData> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    
    /* This method is allways called when an answer is given, either wrong or right.
     * It stops the timer that records how long the person took to answer the question.
     */
    private void Answer()
    {
        currentWord.AddSeenTime(timer.StopTiming());
    }

    /*
	 * This method decides what happens when the button with the correct answer is clicked
	 * It removes the current word from the todolist and disables the buttons for a small while.
	 */
    private void CorrectAnswer()
    {
        Answer();
        currentWord.Solved();
        toDoList.Remove(currentWord);
        StartCoroutine(DisableButtons(correctWaitTime,1));
        InstantiateGame();
    }

	/*
	 * This method decides what happens when the button with the wrong answer is clicked.
	 * It removes the current word from the todolist and inserts it again at the end of the list.
	 * The method then disables the buttons for a while.
	 */
    private void WrongAnswer()
    {
        Answer();
        currentWord.Wrong();
        toDoList.Remove(currentWord);
		toDoList.Add(currentWord);
        StartCoroutine(DisableButtons(wrongWaitTime,0));
        InstantiateGame();
    }

    public void RightPressed()
    {
        if (fastClickFixer)
        {
            fastClickFixer = false;
            numberOfClicks++;
            if (correct == 1)
            {
                CorrectAnswer();
            } else
            {
                WrongAnswer();
            }
        }
		
    }

    public void LeftPressed()
    {
        if (fastClickFixer)
        {
            fastClickFixer = false;
            numberOfClicks++;
            if (correct == 0)
            {
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }
    }

	/* this method disables the buttons after one of then is clicked, 
	 * so that buttons cannot be clicked for a certain amount of time */
    IEnumerator DisableButtons(float time, int correct)
    {
		middleText.GetComponent<UpdateMiddleText>().DisableButtons(correct);
        yield return new WaitForSeconds(time);
        middleText.GetComponent<UpdateMiddleText>().EnableButtons();
        fastClickFixer = true;
        UpdateGame();
    }

    public void SendTimingData()
    {
        GameObject zd = GameObject.FindWithTag("ZeeguuData");

        if (zd == null)
        {
            Debug.Log("Failed finding ZeeguuData object for sending timing data");
            return;
        }

        foreach (WordData w in totalWordList)
        {
            uint timeInMilliseconds = System.Convert.ToUInt32(1000 * w.GetSeenTime());
            zd.GetComponent<ZeeguuData>().sendResults(w.GetID(), "Correct", timeInMilliseconds);
        }
    }

    public override void CreateEndscreen()
    {
        // Is this the correct place?
        // if (!usingHardcodedSet) SendTimingData();

        DrawEndCanvas dec = GetComponent<DrawEndCanvas>();
        dec.EndScreen();
    }
}
