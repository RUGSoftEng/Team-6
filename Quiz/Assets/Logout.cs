using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class Logout : MonoBehaviour {

	public void SignOut() {
		if(File.Exists(Application.persistentDataPath + "bookmarks")) {
			File.Delete(Application.persistentDataPath + "bookmarks");
		}
		
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ZeeguuData")){
            Destroy(obj);
        }

        ZeeguuData.destroySession();
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene(0);
    }
}
