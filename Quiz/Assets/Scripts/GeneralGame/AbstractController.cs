using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * The controller from all the different games should extend from this AbstractController.
 * It has implemented some basic functionality like using the zeeguu bookmarks.
 */
public abstract class AbstractController : MonoBehaviour {

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
            totalWordList.Add(new WordData("Lion", "Leeuw", "A Lion Roars"));
            totalWordList.Add(new WordData("Shout", "Schreeuw", "Harry Shouts to Mary"));
            totalWordList.Add(new WordData("Surf", "Surfen", "Harold loves to surf"));
            totalWordList.Add(new WordData("Moan", "Zeuren", "Jimmy moans a lot"));
        }
        else
        {
            List<Bookmark> localBookmarkList = new List<Bookmark>(zeeguuList[0].GetComponent<ZeeguuData>().userBookmarks);
            for (int i = 0; i < maxAmountOfWords; i++)
            {
                if (localBookmarkList.Count == 0)
                {
                    break;
                }
                int randomIndex = Random.Range(0, localBookmarkList.Count);
                totalWordList.Add(new WordData(localBookmarkList[randomIndex].word, localBookmarkList[randomIndex].translation, localBookmarkList[randomIndex].context));
                localBookmarkList.RemoveAt(randomIndex);
            }
        }
    }

    /* this method should always be called if a game is quit */
    public void Exit()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        this.GetComponent<LoadNewLevel>().LoadLevel();
    }
}
