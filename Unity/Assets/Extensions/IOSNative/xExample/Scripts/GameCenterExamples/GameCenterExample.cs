////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCenterExample : BaseIOSFeaturePreview {
	
	public int hiScore = 100;

	
	private string leaderBoardId =  "your.leaderbord1.id.here";
	private string leaderBoardId2 = "your.leaderbord2.id.here";


	private string TEST_ACHIEVEMENT_1_ID = "your.achievement.id1.here";
	private string TEST_ACHIEVEMENT_2_ID = "your.achievement.id2.here";

	private static bool IsInited = false;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	


	void Awake() {
		if(!IsInited) {
			
			//Achievement registration. If you will skipt this step GameCenterManager.achievements array will contain only achievements with reported progress 
			GameCenterManager.registerAchievement (TEST_ACHIEVEMENT_1_ID);
			GameCenterManager.registerAchievement (TEST_ACHIEVEMENT_2_ID);
			
			
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

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	void OnGUI() {


		UpdateToStartPos();

		if(GameCenterManager.player != null) {
			GUI.Label(new Rect(100, 10, Screen.width, 40), "ID: " + GameCenterManager.player.playerId);
			GUI.Label(new Rect(100, 20, Screen.width, 40), "Name: " +  GameCenterManager.player.displayName);
			GUI.Label(new Rect(100, 30, Screen.width, 40), "Alias: " +  GameCenterManager.player.alias);

		
			//avatar loading will take a while after the user is connectd
			//so we will draw it only after instantiation to avoid null pointer check
			//if you whant to know exaxtly when the avatar is loaded, you can subscribe on 
			//GameCenterManager.GAME_CENTER_USER_INFO_LOADED  			
			//GameCenterManager.GAME_CENTER_USER_INFO_FAILED_TO_LOAD  
			//events and checl for a spesific user ID
			if(GameCenterManager.player.avatar != null) {
				GUI.DrawTexture(new Rect(10, 10, 75, 75), GameCenterManager.player.avatar);
			}
		}

		StartY+= YLableStep;
		StartY+= YLableStep;
		StartY+= YLableStep;


		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "GameCneter Leaderboards", style);


		StartY+= YLableStep;
		
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Leaderboards")) {
			GameCenterManager.showLeaderBoards ();
		}


		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Leader Board1")) {
			GameCenterManager.showLeaderBoard(leaderBoardId);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Report Score LB 1")) {
			hiScore++;
			GameCenterManager.reportScore(hiScore, leaderBoardId);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Score LB 1")) {
			GameCenterManager.getScore(leaderBoardId);
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Leader Board2")) {
			GameCenterManager.showLeaderBoard(leaderBoardId2);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Leader Board2 Today")) {
			GameCenterManager.showLeaderBoard(leaderBoardId2, GCBoardTimeSpan.TODAY);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Report Score LB2")) {
			hiScore++;
			GameCenterManager.reportScore(hiScore, leaderBoardId2);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Score LB 2")) {
			GameCenterManager.getScore(leaderBoardId2);
		}

		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+= YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "GameCneter Achievements", style);

		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Achievements")) {
			GameCenterManager.showAchievements();
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Reset Achievements")) {
			GameCenterManager.resetAchievements();
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Submit Achievements1")) {
			GameCenterManager.submitAchievement(GameCenterManager.getAchievementProgress(TEST_ACHIEVEMENT_1_ID) + 2.432f, TEST_ACHIEVEMENT_1_ID);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Submit Achievements2")) {
			GameCenterManager.submitAchievement(88.66f, TEST_ACHIEVEMENT_2_ID, false);
		}
		


	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
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
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
