/*
 * This is the main class of
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
public class OrderGameController : AbstractController
{
    private List<GameObject> buttons, chromeButtons;
    private Timer timer;
    public Color colorNormal, colorCorrect, colorWrong, colorSelected;
    private List<WordData> sentenceToDo;
    private WordData currentSentence;
    public GameObject end;
    private string [] fragments= new string[4];

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        timer = this.GetComponent<Timer>();
        LoadData();
        sentenceToDo = new List<WordData>(totalWordList);
        updateGame();
    }

    private void updateGame()
    {
        if (sentenceToDo.Count < 1)
        {
            CreateEndscreen();
            return;
        }
        splitSentence(currentSentence);
            
    }

    private void splitSentence(WordData sentence)
    {
        string description = sentence.GetDesc();
        char[] descriptionArr = description.ToLower().ToCharArray();
        int splitLength; //sentence will be split in four parts
        int numSpaces = 0;
        List<int> spaceIndices = new List<int>();
        spaceIndices.Add(0);

        for (int i = 0; i < descriptionArr.Length; i++)  //find spaces
        {
            if (char.IsWhiteSpace(descriptionArr[i]))
            {
                numSpaces++;
                spaceIndices.Add(i);
            }
        }

        splitLength = numSpaces / 4;

        if (splitLength > 0)
        {
            int k = 0;
            int j;
            string str;
            for (j=0; j < numSpaces; j = +splitLength)
            {
                str = new string(descriptionArr, spaceIndices[j], spaceIndices[j + splitLength]);
                fragments[k] = str;
                k++;
            }
            str = new string(descriptionArr, spaceIndices[j], descriptionArr.Length);
            fragments[3] = str;
        }
    }


    public override void CreateEndscreen()
    {
        throw new NotImplementedException();
    }
}