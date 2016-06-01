using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
 * The controller from all the different games should extend from this AbstractController.
 * It has implemented some basic functionality like using the zeeguu bookmarks.
 */
public abstract class AbstractController : MonoBehaviour {

    public Camera mainCamera;
    public int maxAmountOfWords;
    public List<WordData> totalWordList;
    public bool usingHardcodedSet = false;

	/*
	 * The loadData method selects the words to be used in the game.
	 */
	protected void LoadData()
	{
		totalWordList = new List<WordData>();
		GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");
		if (zeeguuList.Length < 1)
		{
            usingHardcodedSet = true;
            Debug.Log("No zeeguuData Available, using hardcoded Set");
			totalWordList.Add(new WordData("Lion", "Leeuw", "The zombie-lion Roars"));
			totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts while he is being ripped in pieces by Mary"));
			totalWordList.Add(new WordData("A super long english word which you would never expect since it is way to long to be writable, or even readible", "Een ontezettend lang engels woord dat je nooit verwacht, het is zo lang dat je hem niet eens kan schrijven laat staan lezen", "Who that was really long"));
			totalWordList.Add(new WordData("Moan", "Zeuren", "Jimmy moans a lot, and this becomes a very long test description so we can check if that also works properly at every game, because it would be very disappointing if it doesn't"));
			totalWordList.Add(new WordData("Manatee", "Zeekoe", "Today I've eaten a manatee for breakfast"));
			totalWordList.Add(new WordData("Moon", "Maan", "Harry moons at Mary"));
			totalWordList.Add(new WordData("Mother", "Moeder", "Last night your mother was really nice to me!"));
		}
		else
		{
            ZeeguuData zd = zeeguuList[0].GetComponent<ZeeguuData>();
            
            
			foreach (Bookmark b in zd.SelectWords(maxAmountOfWords))
            {
                totalWordList.Add(new WordData(b.word, b.translation, b.context, b.id));
            }
        }
    }

    protected void LoadData(int maxWordLength)
    {
        totalWordList = new List<WordData>();
        GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");
        if (zeeguuList.Length < 1)
        {
            LoadData();
        }
        else
        {
            int toLong = 0, prevToLong = 0;
            ZeeguuData zd = zeeguuList[0].GetComponent<ZeeguuData>();

            foreach (Bookmark b in zd.SelectWords(maxAmountOfWords))
            {
                if (b.word.Length <= maxWordLength)
                {
                    totalWordList.Add(new WordData(b.word, b.translation, b.context));
                } else
                {
                    Debug.Log("TOOOO LONG" + b.word);
                    toLong++;
                }
            }

            //to fill up the amount of words that where to long
            while (toLong > prevToLong) {
                int rememberLength = toLong;
                List<Bookmark> bookList = zd.SelectWords(maxAmountOfWords+toLong);
                Debug.Log("BooklistSize: " + bookList.Count + " maxAmountOfWords" + maxAmountOfWords + " toLong: " + toLong);
                for (int i=prevToLong; i<rememberLength; i++)
                {
                    Debug.Log(i);
                    Bookmark b = bookList[maxAmountOfWords+i];
                    if (b.word.Length <= maxWordLength)
                    {
                        Debug.Log("adding it" + b.word);
                        totalWordList.Add(new WordData(b.word, b.translation, b.context));
                    }
                    else
                    {
                        Debug.Log("nog steeds te long" + b.word);
                        toLong++;
                    }
                }
                prevToLong = rememberLength;
                Debug.Log("New Round prevToLong" + prevToLong + " toLong: " + toLong);
            }
            foreach (WordData wd in totalWordList)
            {
                Debug.Log("WORD: " + wd.GetWord());
            }
            Debug.Log(totalWordList);
        }
    }

    /* this method should always be called if a game is quit */
    public void Exit()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        this.GetComponent<LoadNewLevel>().LoadLevel(1);
    }

    abstract public void CreateEndscreen();

    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
