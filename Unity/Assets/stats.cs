using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {

	public string statName;
	public string statText;
	public GUIStyle textStyle;

	public bool isLeft = true;

	public string text = "0.00";
	public int yVal = 50;

	private int score = 0;

	// GAMECENTER
	public string leaderBoardId =  "";
	private static bool IsInited = false;

	void Awake() {
		if(!IsInited) {

			//Listen for the Game Center events
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_LOADED, OnAchievementsLoaded);
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENT_PROGRESS, OnAchievementProgress);
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_RESET, OnAchievementsReset);

			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADER_BOARD_SCORE_LOADED, OnLeaderBoarScoreLoaded);
			
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTICATED, OnAuth);
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTIFICATION_FAILED, OnAuthFailed);

			//Initializing Game Cneter class. This action will triger authentication flow
			GameCenterManager.init();
			IsInited = true;
		}	
	}


	// Use this for initialization
	void Start () {
		textStyle.alignment = TextAnchor.MiddleCenter;

		if(statName.Equals("tapsSec")){
			text = ((float)PlayerPrefs.GetInt("totalTimePlayed") / PlayerPrefs.GetInt("totalTaps")).ToString();
			score = (int)((float)PlayerPrefs.GetInt("totalTimePlayed") / PlayerPrefs.GetInt("totalTaps"));
		}else if(statName.Equals("reactionTimeBest")){
			text = PlayerPrefs.GetFloat(statName).ToString();
			score = (int)PlayerPrefs.GetFloat(statName);
		}else{
			text = PlayerPrefs.GetInt(statName).ToString();
			score = PlayerPrefs.GetInt(statName);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		float div = 0;
		if(isLeft){
			div = Screen.width / 4 - 100;
		}else{
			div = (Screen.width / 4) * 3 - 100;
		}

		textStyle.fontSize = 20;
		GUI.Label(new Rect(div, (Screen.height / 100) * yVal, 200, 20), statText, textStyle);
		textStyle.fontSize = 40;
		GUI.Label(new Rect(div, (Screen.height / 100) * yVal + ((Screen.height / 100) * 7), 200, 20), text, textStyle);
	}

	void OnMouseDown() {
		// submit highscore
		if(score != 0 && score != null){
			GameCenterManager.reportScore(score, leaderBoardId);
		}


		this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	}
	
	void OnMouseUp() {
		this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);


		// Load scoreboard for this stat
		GameCenterManager.showLeaderBoard(leaderBoardId);
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private void OnAchievementsLoaded() {
		Debug.Log ("Achievemnts was loaded from IOS Game Center");
		
		foreach(AchievementTemplate tpl in GameCenterManager.achievements) {
			Debug.Log (tpl.id + ":  " + tpl.progress);
		}
	}
	
	private void OnAchievementsReset() {
		Debug.Log ("All  Achievemnts was reseted");
	}
	
	private void OnAchievementProgress(CEvent e) {
		Debug.Log ("OnAchievementProgress");
		
		AchievementTemplate tpl = e.data as AchievementTemplate;
		Debug.Log (tpl.id + ":  " + tpl.progress.ToString());
	}
	
	private void OnLeaderBoarScoreLoaded(CEvent e) {
		LeaderBoardScoreData data = e.data as LeaderBoardScoreData;
		IOSNativePopUpManager.showMessage("Leader Board " + data.leaderBoardId, "Score: " + data.leaderBoardScore + "\n" + "Rank:" + data.GetRank());
	}
	
	
	private void OnAuth() {
		IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.player.playerId + "\n" + "Alias: " + GameCenterManager.player.alias);
	}
	
	private void OnAuthFailed() {
		IOSNativePopUpManager.showMessage("Game Cneter ", "Player auntification failed");
		
		//if you got this event it means that player canseled auntification flow. With probably mean that playr do not whant to use gamcenter in your game
		
		
	}
}
