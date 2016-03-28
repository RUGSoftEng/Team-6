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
    public Button button;
    public GameObject c;
    public GameObject self;
    public GameObject leftDistance;
    public Color colorCorrect, colorToDo, colorWrong;
    public float height;
    private float count = -1;

	/*
	 * The ongui method creates the buttons of the progress bar in their todo color in the right place.
	 */
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
                butCol.SetColors(colorCorrect, colorToDo, colorWrong);
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
