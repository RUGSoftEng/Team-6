using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;

public class QuizController : MonoBehaviour {

    public Text middleText;
    public float correctWaitTime;
    public float wrongWaitTime;
    public List<WordData> totalWordList;
    public List<WordData> toDoList;
    private WordData currentWord;
    private int correct; //0 is left   1 is right
	public int numberOfClicks;

    void Start()
    {
		numberOfClicks = 0;
		
        totalWordList = new List<WordData>();

        totalWordList.Add(new WordData("Lion", "Leeuw", "A Lion Roars"));
        totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts to Mary"));
        totalWordList.Add(new WordData("Surf", "Surfen", "Harold loves to surf"));
        totalWordList.Add(new WordData("Couch", "Bank", "You can sit on a couch"));
        totalWordList.Add(new WordData("Lighter", "Aansteker", "Light the fire with a lighter"));
        totalWordList.Add(new WordData("Joke", "Grap", "Julie makes a funny joke"));
        toDoList = new List<WordData>(totalWordList);
		Shuffle();
        UpdateGame();
    }

	public void Shuffle()
	{
		System.Random rng = new System.Random();
		int n = toDoList.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(0,n-1);
			WordData value = totalWordList[k];
			totalWordList[k] = totalWordList[n];
			totalWordList[n] = value;
			value = toDoList[k];
			toDoList[k] = toDoList[n];
			toDoList[n] = value;
		}
	}

    private void UpdateGame()
    {
        if (toDoList.Count < 1)
        {
            Debug.Log("Finished, You did it!!!");
            return;
        }

        currentWord = toDoList[0];
        string wrongTrans;
        do
        {
            wrongTrans = SelectRandomWord(totalWordList).GetTrans();
        } while (currentWord.GetTrans().Equals(wrongTrans));
        correct = Random.Range(0, 2);
        middleText.GetComponent<UpdateMiddleText>().UpdateText(currentWord.GetWord(), currentWord.GetTrans(), wrongTrans, correct);
    }

    private WordData SelectRandomWord(List<WordData> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private void CorrectAnswer()
    {
        toDoList.Remove(currentWord);
        StartCoroutine(DisableButtons(correctWaitTime));
    }

    private void WrongAnswer()
    {
        toDoList.Remove(currentWord);
		toDoList.Add(currentWord);
		StartCoroutine(DisableButtons(wrongWaitTime));
    }

    public void RightPressed()
    {
		numberOfClicks++;
        if (correct == 1)
        {
            CorrectAnswer();
        } else
        {
            WrongAnswer();
        }
    }

    public void LeftPressed()
    {
		numberOfClicks++;
        if (correct == 0)
        {
            CorrectAnswer();
        } else
        {
            WrongAnswer();
        }
    }

    IEnumerator DisableButtons(float time)
    {
        middleText.GetComponent<UpdateMiddleText>().DisableButtons();
        yield return new WaitForSeconds(time);
        middleText.GetComponent<UpdateMiddleText>().EnableButtons();
        UpdateGame();
    }
}
