////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class LeaderBoardScoreData  {

	public string leaderBoardId;

	public string rank;
	public string leaderBoardScore;


	public float GetFloatScore() {
		return System.Convert.ToSingle (leaderBoardScore);
	}

	public int GetIntScore() {
		return System.Convert.ToInt32 (leaderBoardScore);
	}


	public int GetRank() {
		return System.Convert.ToInt32 (rank);
	}
}
