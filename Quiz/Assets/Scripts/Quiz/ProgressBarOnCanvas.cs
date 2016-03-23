using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressBarOnCanvas : MonoBehaviour {
    public Canvas canvas;
    public Button button;
    public GameObject c;
    public GameObject self;
    public GameObject leftDistance;
    public Color colorCorrect, colorToDo;
    public float height;
    private float count = -1;

    void OnGUI()
    {
        if (count == -1)
        {
            QuizController controller = c.GetComponent<QuizController>();
            count = controller.totalWordList.Count;
            for (int i = 0; i < count; i++)
            {
                Button newButton = Instantiate(button) as Button;
                SwitchColor butCol = newButton.GetComponent<SwitchColor>();
                butCol.SetColors(colorCorrect, colorToDo);
                controller.totalWordList[i].setObserver(butCol);
                newButton.transform.SetParent(canvas.transform, false);
                RectTransform buttonTrans = newButton.GetComponent<RectTransform>();
                Transform parantTrans = self.GetComponent<Transform>();
                Transform leftTrans = leftDistance.GetComponent<Transform>();
                float distanceBetween = leftTrans.position.x * -2 / count;
                buttonTrans.position = new Vector3(distanceBetween*(i-(count-1)/2f), parantTrans.position.y, 0);
                buttonTrans.sizeDelta = new Vector2((Screen.width / count), height);
            }
        }
        
    }
}
