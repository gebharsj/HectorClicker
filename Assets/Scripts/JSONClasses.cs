using UnityEngine;
using System.Collections.Generic;

public class JSONClasses{
	
}

[System.Serializable]
public class TopTenScore
{
	public string PlayerName;
	public int Score;
}

[System.Serializable]
public class Score
{
	public int ScoreID;
	public int PlayerID;
	public int Score1;

	public Score(int _playerID, int _score1)
	{
		PlayerID = _playerID;
		Score1 = _score1;
	}
}

[System.Serializable]
public class Player
{
	public int PlayerID;
	public string PlayerName;

	public Player(string _playerName)
	{
		PlayerName = _playerName;
	}
}