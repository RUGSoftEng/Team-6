using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ZeeguuData : MonoBehaviour {
    public static string DEFAULT_SERVER = "https://www.zeeguu.unibe.ch/";
    //public static string DEFAULT_SERVER = "http://217.120.38.26:8128";
    public string sessionID;

    public string username;
    public List<Bookmark> userBookmarks;
    public string userNativeLanguage;
    public string userLearnedLanguage;

    public bool loggedIn;

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
        if (!loginRequest.text.Equals("")){
            sessionID = loginRequest.text;
            GetNativeLanguage();
            GetLearnedLanguage();
            GetBookmarks();
            loggedIn = true;
            Application.LoadLevel(1);
        } else {
            Debug.Log("TODO (Rene): Implement something which signals login failure.");
        }
    }

    public IEnumerator NativeLanguageRequest() {

        WWW nativeLanguageRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
        yield return nativeLanguageRequest;
        if (!nativeLanguageRequest.text.Equals("")) {
            userNativeLanguage = nativeLanguageRequest.text;
        }
    }

    public IEnumerator LearnedLanguageRequest() {

        WWW learnedLanguageRequest = new WWW(serverURL + "/learned_language?session=" + sessionID);
        yield return learnedLanguageRequest;
        if (!learnedLanguageRequest.text.Equals("")) {
            userLearnedLanguage = learnedLanguageRequest.text;
        }
    }

    public IEnumerator BookmarksRequest() {
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

    public void GetNativeLanguage() {
        StartCoroutine(NativeLanguageRequest());
    }
    public void GetLearnedLanguage() {
        StartCoroutine(LearnedLanguageRequest());
    }

    public void GetBookmarks() {
        StartCoroutine(BookmarksRequest());
    }
}
