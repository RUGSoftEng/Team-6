/*
 * This script is responsible for handling the communication between the application
 * and the Zeeguu web service API. It is instantiated once at the login screen, and will
 * for the remainder of runtime.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Boomlagoon.JSON;
using System.Linq;

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
    public GameObject loginForm;
    public GameObject signingIn;
    public GameObject loadAnimation;
    GameObject load;
    public FrequencyList frequencyList;

    public string serverURL;

    public void Start() {
        // Find loading animation prefab.
        loadAnimation = GameObject.FindGameObjectWithTag ("ReloadMessage");
        if (loadSession()) {
            //Show "Signing in"
            loginForm.SetActive(false);
            // signingIn.SetActive(true);
            StartCoroutine(LoginRequest("", ""));
        } else {
            //Show the login form
            loginForm.SetActive(true);
            loginForm.GetComponent<Animator>().Play("FoldOut");
            signingIn.SetActive(false);
        }
    }

    IEnumerator TestSession() {
        if (sessionID != 0) {
            WWW testRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
            yield return testRequest;
            if (testRequest.text.Contains("401") || testRequest.text.Equals("")) {
                destroySession();
                sessionID = 0;
            }
        }
    }

    IEnumerator SignIn(string username, string password) {
        if (sessionID == 0) {
            WWWForm loginForm = new WWWForm();
            loginForm.AddField("password", password);

            WWW loginRequest = new WWW(serverURL + "/session/" + username, loginForm);
            yield return loginRequest;

            //The login request either returns a number or some HTML with a HTTP 401 in it.
            if (!System.Int32.TryParse(loginRequest.text, out sessionID)) {
                //Login button shakes and turns red to signal login failure.
                loginButton.GetComponent<Animator>().Play("Disabled");
                loginFail();
            }
        }
    }

    IEnumerator NativeLanguageRequest() {
        WWW nativeLanguageRequest = new WWW(serverURL + "/native_language?session=" + sessionID);
        yield return nativeLanguageRequest;
        if (!nativeLanguageRequest.text.Equals("")) {
            userNativeLanguage = nativeLanguageRequest.text;
        }
    }

    IEnumerator LearnedLanguageRequest() {
        WWW learnedLanguageRequest = new WWW(serverURL + "/learned_language?session=" + sessionID);
        yield return learnedLanguageRequest;
        if (!learnedLanguageRequest.text.Equals("")) {
            userLearnedLanguage = learnedLanguageRequest.text;
        }
    }

    IEnumerator RetrieveBookmarks() {
        if (loadBookmarks()) {
            DateTime lastModified = File.GetLastWriteTime(Application.persistentDataPath + "bookmarks");

            WWWForm bookmarksForm = new WWWForm();
            bookmarksForm.AddField("with_context", "true");
            bookmarksForm.AddField("after_date", lastModified.ToString("s"));

            Debug.Log("Sending POST request:" + (serverURL + "/bookmarks_by_day?session=" + sessionID));
            Debug.Log("With after_date set to: " + lastModified.ToString("s"));

            WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day?session=" + sessionID, bookmarksForm);
            yield return bookmarkRequest;

            if (!bookmarkRequest.text.Equals("")) {
                List<Bookmark> newBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
                Debug.Log(newBookmarks.Count + "additional bookmarks retrieved");
                userBookmarks.AddRange(newBookmarks);

                if(newBookmarks.Count > 0) {
                    yield return RetrieveLearnedBookmarks();
                }

                saveBookmarks();
            }
        } else {
            WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
            yield return bookmarkRequest;
            if (!bookmarkRequest.text.Equals("")) {
                userBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
                Debug.Log("Got all " + userBookmarks.Count + "bookmarks from Zeeguu");
                saveBookmarks();
            }
        }
    }

    void loginFail() {
        Debug.Log("Login failed");
        //Invoked if the login fails.

        loginForm.SetActive(true);
        loginButton.GetComponent<Animator>().Play("Disabled");

        // Finalise loading animation.
        Debug.Log("Destroying load animation");
        Destroy(load);
    }

    //Selects a number of the most useful bookmarks to learn.
    public List<Bookmark> SelectWords(int num) {
        List<Bookmark> selectedWords = new List<Bookmark>();

        if (userBookmarks.Count < num) {
            // If there aren't enough words anyway, don't bother doing the hard computation
            // to select them: just return everything
            return userBookmarks;
        }

        // We need to normalize the timestamps: when we order the bookmarks, they're
        // spread from old to new in values 0.0 to 1.0
        DateTime firstBookmarkDate = userBookmarks.Min(x => x.bookmarkDate);
        DateTime lastBookmarkDate = userBookmarks.Max(x => x.bookmarkDate);
        double dateRange = lastBookmarkDate.Ticks - firstBookmarkDate.Ticks;

        //LINQ is pretty cool yo
        //This picks all learned words, ordered from new to old and common to rare (how to combine these is, ehm, "under construction")
        selectedWords = userBookmarks.Where(x => x.isLearned)
                                    .OrderByDescending(x => (x.bookmarkDate - firstBookmarkDate).Ticks/dateRange + frequencyList.Search(x.word))
                                    .ToList();
        
        if (selectedWords.Count > num) {
            //If we have too many words, just pick the best ones.
            selectedWords = selectedWords.GetRange(0, num);
        } else if (selectedWords.Count < num) {
                //Not enough learned words: supplement with non-learned words, again preferring the newest most common ones.
                selectedWords.AddRange(userBookmarks.Where(x => !x.isLearned)
                                                    .OrderBy(x => (x.bookmarkDate - firstBookmarkDate).Ticks / dateRange + frequencyList.Search(x.word))
                                                    .ToList()
                                                    .GetRange(0,num- selectedWords.Count));
        }

        return selectedWords;
    }

    //Marks per bookmark in userBookmarks whether it is being learned or not.
    IEnumerator RetrieveLearnedBookmarks() {
        WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
        yield return bookmarkRequest;

        JSONArray learnedBookmarks = JSONArray.Parse(bookmarkRequest.text);

        foreach(Bookmark bm in userBookmarks) {
            bm.isLearned = false;
            foreach(JSONValue lbm in learnedBookmarks) {
                if (lbm.Obj.GetString("id") == bm.id.ToString()) {
                    bm.isLearned = true;
                    break;
                }
            }
        }
    }
    
    IEnumerator LoginRequest(string username, string password){
        // Instantiate loading animation.
        Debug.Log ("Instantiating load animation");
        GameObject canvas = GameObject.FindGameObjectsWithTag ("canvas")[0];
        load = Instantiate (loadAnimation);
        load.transform.SetParent (canvas.transform);
        
        yield return TestSession();
        
        yield return SignIn(username,password);

        Debug.Log(sessionID);

        if (sessionID == 0) {
            loginFail();
        } else {
            yield return NativeLanguageRequest();
        }

        Debug.Log(sessionID);
        
        if(userNativeLanguage != null && userNativeLanguage != "") {
            yield return LearnedLanguageRequest();
        } else {
            loginFail();
        }

        if (userLearnedLanguage != null && userLearnedLanguage != "") {
            yield return RetrieveBookmarks();
        } else {
            loginFail();
        }

        if(userBookmarks == null) {
            loginFail();
        }

        // Store the session if needed.
        if (keepSignedIn.isOn) {
            saveSession();
        } else {
            destroySession();
        }
        
        //Loading up the word frequency list to be used in word selection
        frequencyList = new FrequencyList(userLearnedLanguage);
        frequencyList.initialize();
        
        // Finalise loading animation.
        Debug.Log ("Destroying load animation");
        Destroy (load);

        Debug.Log(frequencyList.lang);
        //Go to main menu
        if(frequencyList.lang != null && frequencyList.lang != "") {
            SceneManager.LoadScene(1);
        } else {
            loginFail();
        }
        
    }

    // This function will update the bookmarks. Nothing more, nothing less.
    public IEnumerator UpdateBookmarks (GameObject animation) {
        GameObject button = GameObject.FindGameObjectWithTag ("ReloadButton");
        button.GetComponent<Button>().interactable = false;

        Debug.Log ("Instantiating load animation");
        GameObject canvas = GameObject.FindGameObjectsWithTag ("canvas")[0];
        GameObject load = Instantiate (animation);
        load.transform.SetParent (canvas.transform);

        yield return RetrieveBookmarks();

        Debug.Log ("Destroying load animation");
        Destroy (load);
        
        button.GetComponent<Button> ().interactable = true;
    }

    IEnumerator BookmarksRequest() {
        WWW bookmarkRequest = new WWW(serverURL + "/bookmarks_by_day/with_context?session=" + sessionID);
        yield return bookmarkRequest;
        if (!bookmarkRequest.text.Equals("")) {
            userBookmarks = Bookmark.ListFromJson(bookmarkRequest.text);
        }
    }

    public void Login(GameObject animation){
        loginForm.GetComponent<Animator>().Play("FoldOut");
        loginForm.SetActive(false);
        loadAnimation = animation;
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

    // Serializes a session and saves it to a binary file.
    private void saveSession() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "session");
        formatter.Serialize(file, new SavedSession(sessionID,username,serverURL));
        file.Close();
    }

    // Loads and deserializes a saved session from file.
    // Returns true on success, false if session file non-existent.
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

    // Loads the list of bookmarks from file.
    // Returns true on success, false if bookmark file does not exist.
    private bool loadBookmarks() {
        if(File.Exists(Application.persistentDataPath + "bookmarks")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "bookmarks", FileMode.Open);
            userBookmarks = (List<Bookmark>)formatter.Deserialize(file);
            Debug.Log(userBookmarks.Count + " saved bookmarks found");
            file.Close();
            Debug.Log("first bookmark was bookmarked at timestamp " + userBookmarks[0].bookmarkDate.Ticks);
            if(userBookmarks.Count > 0 && userBookmarks[0].bookmarkDate.Ticks == 0) {
                return false;
            }
            return true;
        } else {
            Debug.Log("No bookmarks saved");
            return false;
        }
    }

    private void saveBookmarks() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "bookmarks");
        formatter.Serialize(file, userBookmarks);
        Debug.Log(userBookmarks.Count + " bookmarks saved");
        file.Close();
    }

    public static void destroySession() {
        if (File.Exists(Application.persistentDataPath + "session")) {
            File.Delete(Application.persistentDataPath + "session");
        }
    }

    // Function that redirects to the website for sign-up.
    public void SignUp ()
    {
        Application.OpenURL("https://www.zeeguu.unibe.ch/");
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
