/*
 * This class is the main controller of the quiz game. It decides what words are displayed.
 *
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuizController : AbstractController {

    public Text middleText;
    public Text DescriptionShower;
    public float correctWaitTime;
    public float wrongWaitTime;
    public List<WordData> toDoList;
    private WordData currentWord;
    private int correct; //0 is left   1 is right
	public int numberOfClicks;
    private bool fastClickFixer = true; //To fix the delay of disabeling buttons 
    private Timer timer;
    public GameObject end;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
        numberOfClicks = 0;
        LoadData();
        toDoList = new List<WordData>(totalWordList);
        UpdateGame();
    }

	/*
	 * The UpdateGame method makes sure everything in the game is correct at any point in time.
	 * It sets the right words for the buttons and selects new ones if needed.
	 */
    private void UpdateGame()
    {
        if (toDoList.Count < 1)
        {
			CreateEndscreen(end);
            StartCoroutine(WaitFinished());
			return;
        }

        DescriptionShower.GetComponent<EditText>().removeText();
        currentWord = toDoList[0];
        string wrongTrans;
        do
        {
            wrongTrans = SelectRandomWord(totalWordList).GetTrans();
        } while (currentWord.GetTrans().Equals(wrongTrans));
        correct = Random.Range(0, 2);
        middleText.GetComponent<UpdateMiddleText>().UpdateText(currentWord.GetWord(), currentWord.GetTrans(), wrongTrans, correct);
        timer.StartTiming();
    }
	
	/*
	 * returns a random word from a wordlist.
	 */
    private WordData SelectRandomWord(List<WordData> list)
    {
        return list[Random.Range(0, list.Count)];
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
        DescriptionShower.GetComponent<EditText>().setText(currentWord.GetDesc());
        StartCoroutine(DisableButtons(wrongWaitTime,0));
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
}
