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
			GameObject canvas = GameObject.FindGameObjectsWithTag("canvas")[0];
            AbstractController controller = c.GetComponent<AbstractController>();
            count = controller.totalWordList.Count;
            for (int i = 0; i < count; i++)
            {
                Button newProgressElement = Instantiate(progressElement) as Button;
                SwitchColor sC = newProgressElement.GetComponent<SwitchColor>();
                sC.SetColors(colorCorrect, colorToDo, colorWrong);
                controller.totalWordList[i].setObserver(sC);

                newProgressElement.transform.SetParent(canvas.transform);
				newProgressElement.transform.localScale = new Vector3(1, 1, 1);
                RectTransform rt = newProgressElement.GetComponent<RectTransform>();
				rt.anchorMin = new Vector2(i/count,0F);
				rt.anchorMax = new Vector2((i+1)/count,0.05F);
				rt.offsetMin = new Vector2(0,0);
				rt.offsetMax = new Vector2(0,0);
            }
        }
    }
}
