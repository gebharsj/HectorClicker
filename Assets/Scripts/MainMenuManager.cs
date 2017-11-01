using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenuManager : MonoBehaviour {

	public delegate void StartGameDelegate();
	public static event StartGameDelegate OnGameStart;

	[SerializeField]
	List<GameObject> gameObjects;

	[SerializeField]
	Text nameText;

	string playerName;
	public int PlayerID { get; private set; }

	public void LoadMainMenu()
	{
		ToggleObjects (true);
	}

	public void StartGame()
	{
		ToggleObjects (false);
		StartCoroutine (PostPlayer ());
		OnGameStart ();
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	public void ToggleObjects(bool isActive)
	{
		foreach(GameObject go in gameObjects)
		{
			go.SetActive (isActive);
		}
	}

	public void SetPlayerName()
	{
		playerName = nameText.text;
	}

	IEnumerator PostPlayer()
	{
		Player playerToPost = new Player (playerName);
		UnityWebRequest www = UnityWebRequest.Post("http://localhost:82/api/Players", "");
		UploadHandler customUploadHandler = new UploadHandlerRaw (System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerToPost)));
		customUploadHandler.contentType = "application/json";
		www.uploadHandler = customUploadHandler;
		yield return www.Send ();

		string playerIDString = www.GetResponseHeader ("Location").Substring(32);
		PlayerID = System.Convert.ToInt32 (playerIDString);

		if (www.error != null)
			Debug.Log ("Error: " + www.error);
		else
			Debug.Log ("Success: " + www.responseCode);	
	}
}