using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateMiddleText : MonoBehaviour {

    public Button right;
    public Button left;
    public Color correctCol;
    public Color wrongCol;

    public void UpdateText(string ownStr, string trans, string wrongTrans, int correct)
    {
        this.GetComponent<Text>().text = ownStr;
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        if (correct == 0)
        {
            l.UpdateText(trans);
            l.SetPressedColor(correctCol);
            r.UpdateText(wrongTrans);
            r.SetPressedColor(wrongCol);
        } else
        {
            l.UpdateText(wrongTrans);
            l.SetPressedColor(wrongCol);
            r.UpdateText(trans);
            r.SetPressedColor(correctCol);
        }
    }
}
