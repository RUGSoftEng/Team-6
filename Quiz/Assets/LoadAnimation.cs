using UnityEngine;
using System.Collections;

public class LoadAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RectTransform rt = GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.5F, 0.5F);
        rt.anchorMax = new Vector2 (0.5F, 0.5F); 
        rt.offsetMin = new Vector2 (-90, -76);
        rt.offsetMax = new Vector2 (90, 76);
        rt.localScale = new Vector3 (1, 1, 1);
        rt.localPosition = new Vector3 (0, 40, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * 5);
	}
}
