using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ZeeguuData : MonoBehaviour {
    public static string DEFAULT_SERVER = "https://www.zeeguu.unibe.ch/";
    //public static string DEFAULT_SERVER = "http://217.120.38.26:8128";
    public int sessionID;

    public string username;
    public List<Bookmark> userBookmarks;
    public string userNativeLanguage;
    public string userLearnedLanguage;
    
    public InputField usernameText;
    public InputField passwordText;
    public InputField serverText;
    public GameObject loginButton; //Handle needed so we can trigger animation of the login button.

    public string serverURL;
    
    IEnumerator LoginRequest(string username, string password){
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("password", password);

        WWW loginRequest = new WWW(serverURL + "/session/" + username, loginForm);
        yield return loginRequest;

        //The login request either returns a number or some HTML with a HTTP 401 in it.
        if(!System.Int32.TryParse(loginRequest.text, out sessionID)){
            //Login button shakes and turns red to signal login failure.
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        WWW nativeLanguageRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
        yield return nativeLanguageRequest;
        if (!nativeLanguageRequest.text.Equals("")) {
            userNativeLanguage = nativeLanguageRequest.text;
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        WWW learnedLanguageRequest = new WWW(serverURL + "/learned_language?session=" + sessionID);
        yield return learnedLanguageRequest;
        if (!learnedLanguageRequest.text.Equals("")) {
            userLearnedLanguage = learnedLanguageRequest.text;
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
        yield return bookmarkRequest;
        if (!bookmarkRequest.text.Equals("")) {
            userBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        //Go to main menu
        SceneManager.LoadScene(1);
    }

    IEnumerator BookmarksRequest() {
        WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
        yield return bookmarkRequest;
        if (!bookmarkRequest.text.Equals("")) {
            userBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
        }
    }

    public void Login(){
        username = usernameText.text;
        string password = passwordText.text;
        serverURL = serverText.text.Equals("") ? DEFAULT_SERVER : serverText.text;
        StartCoroutine(LoginRequest(username, password));
    }

    public void Login(string username, string password, string server) {
        serverURL = server.Equals("") ? DEFAULT_SERVER : server;
        StartCoroutine(LoginRequest(username, password));
    }

    public void GetBookmarks() {
        StartCoroutine(BookmarksRequest());
    }
}
