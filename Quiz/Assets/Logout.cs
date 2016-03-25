using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour {

	public void SignOut() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ZeeguuData")){
            Destroy(obj);
        }
        SceneManager.LoadScene(0);
    }
}
