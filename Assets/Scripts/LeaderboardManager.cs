using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {

	[SerializeField]
	List<Text> scoreListings;

	[SerializeField]
	List<GameObject> gameObjects;

	public string targetURL;
	public List<TopTenScore> highScores;

	void OnEnable()
	{
		GameManager.OnGameEnd += LoadLeaderboard;
	}

	void OnDisable()
	{
		GameManager.OnGameEnd -= LoadLeaderboard;
	}

	void LoadLeaderboard()
	{
		ToggleObjects (true);
		StartCoroutine (GetScores ());
	}

	public void ToggleObjects(bool isActive)
	{
		foreach(GameObject go in gameObjects)
		{
			go.SetActive (isActive);
		}
	}
	
	IEnumerator GetScores()
	{
		WWW hectorClickerResponse = new WWW (targetURL);
		yield return hectorClickerResponse;
		if (hectorClickerResponse.error != null)
			Debug.Log (hectorClickerResponse.error);
		else {
			//remove square brackets
			string responseString = hectorClickerResponse.text.Substring(1, hectorClickerResponse.text.Length - 2);
			//split on commas
			string[] substrings = responseString.Split('{');

			//pass each string into from json
			int i = 0;
			foreach (string sub in substrings) {
				//store all of that data in a list of scores
				if(sub != "")
				{
					string tempString = "{" + sub;

					if(tempString[tempString.Length - 1] == ',')
						tempString = tempString.Substring(0, tempString.Length - 1);

					highScores.Add (JsonUtility.FromJson<TopTenScore> (tempString));
					scoreListings [i].text = highScores [i].PlayerName + ": " + highScores [i].Score;
					i++;
				}
			}
		}
		yield return null;
	}
}