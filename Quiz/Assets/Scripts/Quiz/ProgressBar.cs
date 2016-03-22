using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressBar : MonoBehaviour {
	
	private int count;
    private Texture2D texG, texR;
	private GUIStyle style;
	//private QuizController controller;
	
	void OnGUI() {
		GameObject c = GameObject.Find("Controller");
        QuizController controller = c.GetComponent<QuizController>();
		count = controller.totalWordList.Count;
		if (count==0) {
			return;
		}
		int w = (Screen.width-50)/count;
		for (int i=0;i<count;i++) {
			if (controller.toDoList.Contains(controller.totalWordList[i])) {
				GUI.skin.box.normal.background = texR;
			} else {
				GUI.skin.box.normal.background = texG;
			}
			GUI.Box(new Rect(30+w*i,505,w-5,Screen.height-515), GUIContent.none);
		}
    }


	// Use this for initialization
	void Start () {
		texR = new Texture2D(1,1);
		texR.SetPixel(0,0,Color.grey);
		texR.wrapMode = TextureWrapMode.Repeat;
		texR.Apply();
		
		texG = new Texture2D(1,1);
		texG.SetPixel(0,0,Color.green);
		texG.wrapMode = TextureWrapMode.Repeat;
		texG.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
