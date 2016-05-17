using UnityEngine;
using System.Collections;

public class ReloadBookies : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reload () {
		Debug.Log ("Attempting to refresh the bookmarks.");
		GameObject tmp = GameObject.FindGameObjectWithTag ("ZeeguuData");
		ZeeguuData bookshelf = tmp.GetComponent <ZeeguuData> ();
		StartCoroutine (bookshelf.UpdateBookmarks ());
	}
}
