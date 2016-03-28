using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ZeeguuData : MonoBehaviour {
    public static string DEFAULT_SERVER = "https://www.zeeguu.unibe.ch/";
    //public static string DEFAULT_SERVER = "http://217.120.38.26:8128";
    public int sessionID = 0;

    public string username;
    public List<Bookmark> userBookmarks;
    public string userNativeLanguage;
    public string userLearnedLanguage;
    
    public InputField usernameText;
    public InputField passwordText;
    public InputField serverText;
    public Toggle keepSignedIn;
    public GameObject loginButton; //Handle needed so we can trigger animation of the login button.

    public string serverURL;

    public void Start() {
        if (loadSession()) {
            StartCoroutine(LoginRequest("", ""));
        }
    }
    
    IEnumerator LoginRequest(string username, string password){
        //First, try if a stored session exists and if it still works.
        if(sessionID != 0) {
            WWW testRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
            yield return testRequest;
            if (testRequest.text.Contains("401") || testRequest.text.Equals("")) {
                destroySession();
                sessionID = 0;
            }
        }
        
        //If not, log in
        if (sessionID == 0) {
            WWWForm loginForm = new WWWForm();
            loginForm.AddField("password", password);

            WWW loginRequest = new WWW(serverURL + "/session/" + username, loginForm);
            yield return loginRequest;

            //The login request either returns a number or some HTML with a HTTP 401 in it.
            if (!System.Int32.TryParse(loginRequest.text, out sessionID)) {
                //Login button shakes and turns red to signal login failure.
                loginButton.GetComponent<Animator>().Play("Disabled");
                yield break;
            }
        }

        //Retrieve user native language
        WWW nativeLanguageRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
        yield return nativeLanguageRequest;
        if (!nativeLanguageRequest.text.Equals("")) {
            userNativeLanguage = nativeLanguageRequest.text;
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        //Retrieve user learned language
        WWW learnedLanguageRequest = new WWW(serverURL + "/learned_language?session=" + sessionID);
        yield return learnedLanguageRequest;
        if (!learnedLanguageRequest.text.Equals("")) {
            userLearnedLanguage = learnedLanguageRequest.text;
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        //Retrieve user bookmarks
        WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
        yield return bookmarkRequest;
        if (!bookmarkRequest.text.Equals("")) {
            userBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
        } else {
            loginButton.GetComponent<Animator>().Play("Disabled");
            yield break;
        }

        //Store the session if needed.
        if (keepSignedIn.isOn) {
            saveSession();
        } else {
            destroySession();
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

    private void saveSession() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "session");
        formatter.Serialize(file, new SavedSession(sessionID,username,serverURL));
        file.Close();
    }

    private bool loadSession() {
        if(File.Exists(Application.persistentDataPath + "session")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "session", FileMode.Open);
            SavedSession session = (SavedSession)formatter.Deserialize(file);
            file.Close();
            sessionID = session.sessionID;
            username = session.username;
            serverURL = session.server;
            return true;
        } else {
            return false;
        }
    }

    public static void destroySession() {
        if (File.Exists(Application.persistentDataPath + "session")) {
            File.Delete(Application.persistentDataPath + "session");
        }
    }
}

[System.Serializable]
public class SavedSession {
    public int sessionID;
    public string username;
    public string server;

    public SavedSession(int sessionID, string username, string server) {
        this.sessionID = sessionID;
        this.username = username;
        this.server = server;
    }

}
