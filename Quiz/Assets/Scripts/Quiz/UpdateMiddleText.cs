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
            l.SetDisabledColor(correctCol);
            r.UpdateText(wrongTrans);
            r.SetDisabledColor(wrongCol);
        } else
        {
            l.UpdateText(wrongTrans);
            l.SetDisabledColor(wrongCol);
            r.UpdateText(trans);
            r.SetDisabledColor(correctCol);
        }
    }

    public void DisableButtons()
    {
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        r.Disable();
        l.Disable();
    }

    public void EnableButtons()
    {
        UpdateButton r = right.GetComponent<UpdateButton>();
        UpdateButton l = left.GetComponent<UpdateButton>();
        r.Enable();
        l.Enable();
    }
}
