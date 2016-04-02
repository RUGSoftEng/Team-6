/*
 * This class creates a progressbar based on the current totalwordlist and todolist.
 * It uses disabled buttons that are colored green if the corresponding word has been answered correct,
 * red if it was answered wrong and grey if they haven't been answered yet.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressBarOnCanvas : MonoBehaviour {
    public Canvas canvas;
    public Button progressElement;
    public GameObject c;
    public Color colorCorrect, colorToDo, colorWrong;
    private float count = -1;

	/*
	 * The ongui method creates the buttons of the progress bar in their todo color in the right place.
	 */
    void OnGUI()
    {
        if (count == -1)
        {
            QuizController controller = c.GetComponent<QuizController>();
            RectTransform canvasTrans = canvas.GetComponent<RectTransform>();

            count = controller.totalWordList.Count;
            float width = canvasTrans.sizeDelta.x / count;
            float spaceBetween = width*canvasTrans.localScale.x;
            for (int i = 0; i < count; i++)
            {
                Button newProgressElement = Instantiate(progressElement) as Button;
                SwitchColor sC = newProgressElement.GetComponent<SwitchColor>();
                sC.SetColors(colorCorrect, colorToDo, colorWrong);
                controller.totalWordList[i].setObserver(sC);

                newProgressElement.transform.SetParent(this.transform, false);
                RectTransform newTrans = newProgressElement.GetComponent<RectTransform>();
                newTrans.position = new Vector3(spaceBetween*((float)(i-count/2))+0.5f*spaceBetween, newTrans.position.y);//(float)(0.5 * spaceBetween + i * spaceBetween), newTrans.position.y);
                newTrans.sizeDelta = new Vector2(width, newTrans.sizeDelta.y);
            }
        }
    }
}
