using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuizController : MonoBehaviour {

    public Text middleText;
    private List<WordData> totalWordList;
    private List<WordData> toDoList;
    private WordData currentWord;
    private int correct; //0 is left   1 is right

    void Start()
    {
        totalWordList = new List<WordData>();
        
        totalWordList.Add(new WordData("Lion", "Leeuw", "A Lion Roars"));
        totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts to mary"));
        totalWordList.Add(new WordData("Surf", "Surfen", "Harold loves surfing"));
        totalWordList.Add(new WordData("Couch", "Bank", "You can sit on a couch"));
        totalWordList.Add(new WordData("Lighter", "Aansteker", "Light the fire with a lighter"));
        totalWordList.Add(new WordData("Joke", "Grap", "Julie makes a funny joke"));
        toDoList = new List<WordData>(totalWordList);
        UpdateGame();
    }

    private void UpdateGame()
    {
        if (toDoList.Count < 1)
        {
            Debug.Log("Finished, You did it!!!");
            return;
        }

        currentWord = SelectRandomWord(toDoList);
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
    }

    public void RightPressed()
    {
        if (correct == 1)
        {
            CorrectAnswer();
        }
        UpdateGame();
    }

    public void LeftPressed()
    {
        if (correct == 0)
        {
            CorrectAnswer();
        }
        UpdateGame();
    }
}
