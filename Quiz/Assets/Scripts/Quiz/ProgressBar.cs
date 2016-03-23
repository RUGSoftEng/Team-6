using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressBar : MonoBehaviour {
	
	private int count;
    private Texture2D tex;
	public Color colorCorrect,colorToDo,colorBack,colorWrong;
	private GUIStyle style;
	
	void OnGUI() {
		GameObject c = GameObject.Find("Controller");
        QuizController controller = c.GetComponent<QuizController>();
		count = controller.totalWordList.Count;
		if (count==0) {
			return;
		}
		tex.SetPixel(0,0,this.colorBack);
		tex.Apply();
		GUI.skin.box.normal.background = tex;
		GUI.Box(new Rect(25,550,Screen.width-47,Screen.height-555), GUIContent.none);
		int w = (Screen.width-50)/count;
		for (int i=0;i<count;i++) {
			if (controller.toDoList.Contains(controller.totalWordList[i])) {
				if (i<controller.numberOfClicks) {
					tex.SetPixel(0,0,this.colorWrong);
				} else {
					tex.SetPixel(0,0,this.colorToDo);
				}
			} else {
				tex.SetPixel(0,0,this.colorCorrect);
			}
			tex.Apply();
			GUI.skin.box.normal.background = tex;
			GUI.Box(new Rect(30+w*i,555,w-5,Screen.height-565), GUIContent.none);
		}
    }


	// Use this for initialization
	void Start () {
		tex = new Texture2D(1,1);
		tex.SetPixel(0,0,colorToDo);
		tex.wrapMode = TextureWrapMode.Repeat;
		tex.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
