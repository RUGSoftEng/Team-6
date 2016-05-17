using UnityEngine;
using System.Collections;

public class ReloadBookies : MonoBehaviour {

	public GameObject loadAnimation;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reload (GameObject loadAnimation) {
		Debug.Log ("Attempting to refresh the bookmarks.");
		GameObject tmp = GameObject.FindGameObjectWithTag ("ZeeguuData");
		ZeeguuData bookshelf = tmp.GetComponent <ZeeguuData> ();
		StartCoroutine (bookshelf.UpdateBookmarks (loadAnimation));
	}
}
