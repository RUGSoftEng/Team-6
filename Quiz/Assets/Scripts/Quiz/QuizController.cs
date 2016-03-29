/*
 * This class is the main controller of the quiz game. It decides what words are displayed.
 *
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;

public class QuizController : MonoBehaviour {

    public Text middleText;
    public Text DescriptionShower;
    public int maxAmountOfWords;
    public float correctWaitTime;
    public float wrongWaitTime;
    public List<WordData> totalWordList;
    public List<WordData> toDoList;
    private WordData currentWord;
    private int correct; //0 is left   1 is right
	public int numberOfClicks;
    private bool fastClickFixer = true; //To fix the delay of disabeling buttons 

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        numberOfClicks = 0;
        LoadData();
        UpdateGame();
    }

	/*
	 * The loadData method selects the words to be used in the game.
	 */
    private void LoadData()
    {
        totalWordList = new List<WordData>();
        GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");

        if (zeeguuList == null)
        {
            Debug.Log("No zeeguuData Available, using hardcoded Set");
            totalWordList.Add(new WordData("Lion", "Leeuw", "A Lion Roars"));
            totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts to Mary"));
            totalWordList.Add(new WordData("Surf", "Surfen", "Harold loves to surf"));
            totalWordList.Add(new WordData("Couch", "Bank", "You can sit on a couch"));
            totalWordList.Add(new WordData("Lighter", "Aansteker", "Light the fire with a lighter"));
            totalWordList.Add(new WordData("Joke", "Grap", "Julie makes a funny joke"));
            totalWordList.Add(new WordData("Shark", "Haai", "It bites"));
        } else
        {
            List<Bookmark> localBookmarkList = new List<Bookmark>(zeeguuList[0].GetComponent<ZeeguuData>().userBookmarks);
            for (int i=0; i<maxAmountOfWords; i++)
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
        toDoList = new List<WordData>(totalWordList);
    }

	/*
	 * The UpdateGame method makes sure everything in the game is correct at any point in time.
	 * It sets the right words for the buttons and selects new ones if needed.
	 */
    private void UpdateGame()
    {
        if (toDoList.Count < 1)
        {
            Debug.Log("Finished, You did it!!!");
            Exit();
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
    }
	
	/*
	 * returns a random word from a wordlist.
	 */
    private WordData SelectRandomWord(List<WordData> list)
    {
        return list[Random.Range(0, list.Count)];
    }

	/*
	 * This method decides what happens when the button with the correct answer is clicked
	 * It removes the current word from the todolist and disables the buttons for a small while.
	 */
    private void CorrectAnswer()
    {
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

    /* this method should always be called if the quiz game is exitted */
    public void Exit()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        this.GetComponent<LoadNewLevel>().LoadLevel();
    }

    IEnumerator DisableButtons(float time, int correct)
    {
        middleText.GetComponent<UpdateMiddleText>().DisableButtons(correct);
        yield return new WaitForSeconds(time);
        middleText.GetComponent<UpdateMiddleText>().EnableButtons();
        fastClickFixer = true;
        UpdateGame();
    }
}
