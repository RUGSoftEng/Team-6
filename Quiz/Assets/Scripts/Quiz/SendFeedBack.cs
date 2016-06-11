using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SendFeedBack : MonoBehaviour {

    public void Pressed()
    {
        GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");
        GameObject[] controllerList = GameObject.FindGameObjectsWithTag("Controller");

        ZeeguuData zd = zeeguuList[0].GetComponent<ZeeguuData>();
        QuizController qc= controllerList[0].GetComponent<QuizController>();

        WordData wd = qc.GetCurrentWord();

        qc.CorrectAnswer();

        string message = GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().text;

        //zd.SendStringAtWord(wd.GetID(), message);
    }
}
