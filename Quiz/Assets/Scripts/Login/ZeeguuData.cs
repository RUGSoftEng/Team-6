using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ZeeguuData : MonoBehaviour {
    public static string DEFAULT_SERVER = "https://www.zeeguu.unibe.ch/";
    //public static string DEFAULT_SERVER = "http://217.120.38.26:8128";
    public string sessionID;

    public string username;
    public List<string> userWords;
    public string userLanguage;

    public InputField usernameText;
    public InputField passwordText;
    public InputField serverText;

    public string serverURL;

    public Text something;
    
    public IEnumerator LoginRequest(string username, string password){
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("password", password);

        WWW loginRequest = new WWW(serverURL + "/session/" + username, loginForm);
        yield return loginRequest;
        // TODO: What if login fails?
        if (!sessionID.Equals("")){
            sessionID = loginRequest.text;
            Application.LoadLevel(1);
        } else {
            Debug.Log("TODO (Rene): Implement something which signals login failure.");
            sessionID = "nope";
            Application.LoadLevel(1);
        }
    }
    public void Login(){
        username = usernameText.text;
        string password = passwordText.text;
        serverURL = serverText.text.Equals("") ? DEFAULT_SERVER : serverText.text;
        StartCoroutine(LoginRequest(username, password));
    }
}
