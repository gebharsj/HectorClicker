using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

	public delegate void EndGameDelegate();
	public static event EndGameDelegate OnGameEnd;

	[SerializeField]
	MainMenuManager mainMenuManager;

	[SerializeField]
	List<GameObject> gameObjects;

	[SerializeField]
	Text timerText;

	[SerializeField]
	Text scoreText;

	int score;

	void OnEnable()
	{
		MainMenuManager.OnGameStart += StartGame;
	}

	void OnDisable()
	{
		MainMenuManager.OnGameStart -= StartGame;
	}

	void StartGame()
	{
		ToggleObjects (true);
		score = 0;
		scoreText.text = score.ToString();
		StartCoroutine (Timer(120));
	}

	public void AddScore()
	{
		score++;
		scoreText.text = score.ToString ();
	}

	IEnumerator Timer(int time)
	{
		for(int i = time; i > 0; i--)
		{
			timerText.text = i.ToString();
			yield return new WaitForSeconds (1f);
		}

		ToggleObjects (false);
		EndGame ();
	}

	void ToggleObjects(bool isActive)
	{
		foreach(GameObject go in gameObjects)
		{
			go.SetActive (isActive);
		}
	}

	void EndGame()
	{		
		StartCoroutine (PostScores());
	}

	IEnumerator PostScores()
	{
		Score scoreToPost = new Score (mainMenuManager.PlayerID, score);
		UnityWebRequest www = UnityWebRequest.Post("http://localhost:82/api/Scores", "");
		UploadHandler customUploadHandler = new UploadHandlerRaw (System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(scoreToPost)));
		customUploadHandler.contentType = "application/json";
		www.uploadHandler = customUploadHandler;
		yield return www.Send ();
		if (www.error != null)
			Debug.Log ("Error: " + www.error);
		else 
			Debug.Log ("Success: " + www.responseCode);		

		OnGameEnd ();
	}
}