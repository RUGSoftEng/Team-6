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

	/*
	 * The loadData method selects the words to be used in the game.
	 */
	protected void LoadData()
	{
		totalWordList = new List<WordData>();
		GameObject[] zeeguuList = GameObject.FindGameObjectsWithTag("ZeeguuData");
		if (zeeguuList.Length < 1)
		{
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
                totalWordList.Add(new WordData(b.word, b.translation, b.context));
            }

            //List<Bookmark> localBookmarkList = new List<Bookmark>(zeeguuList[0].GetComponent<ZeeguuData>().userBookmarks);        
            /*{
				if (localBookmarkList.Count == 0)
				{
					break;
				}
				int randomIndex = Random.Range(0, localBookmarkList.Count);
				totalWordList.Add(new WordData(localBookmarkList[randomIndex].word, localBookmarkList[randomIndex].translation, localBookmarkList[randomIndex].context));
				localBookmarkList.RemoveAt(randomIndex);
			}*/
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
