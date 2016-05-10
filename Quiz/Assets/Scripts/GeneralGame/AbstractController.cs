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
			totalWordList.Add(new WordData("A super long english word which you would never expect since it is way to long to be writable, or even readible", "Een ontezettend lang engels woord dat je nooit verwacht, het is zo lang dat je hem niet eens kan schrijven laat staan lezen", "Who that was really long"));
			totalWordList.Add(new WordData("Moan", "Zeuren", "Jimmy moans a lot, and this becomes a very long test description so we can check if that also works properly at every game, because it would be very disappointing if it doesn't"));
			totalWordList.Add(new WordData("Manatee", "Zeekoe", "Today I've eaten a manatee for breakfast"));
			totalWordList.Add(new WordData("Moon", "Maan", "The moon comes out at night"));
			totalWordList.Add(new WordData("Sun", "Zon", "The sun sits high in the sky"));
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
		this.GetComponent<LoadNewLevel>().LoadLevel(1);
	}

	abstract public void Continue();
	
	public void CreateEndscreen(GameObject end) {
		GameObject canvas = GameObject.FindGameObjectsWithTag("canvas")[0];
		GameObject endscreen = Instantiate(end);
		endscreen.transform.SetParent(canvas.transform);
		endscreen.transform.localScale = new Vector3(1, 1, 1);
		RectTransform rt = endscreen.GetComponent<RectTransform>();
		rt.anchorMin = new Vector2(0,0);
		rt.anchorMax = new Vector2(1,1);
		rt.offsetMin = new Vector2(0,0);
		rt.offsetMax = new Vector2(0,0);
	}
	
	/* Waits for a mouseclick/fingerpress and then goes back to the menu*/
	public IEnumerator WaitFinished()
	{
		while(true) {
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
				Exit();
				break;
			}
			yield return null;
		}
	}
}
