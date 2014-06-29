using UnityEngine;
using System.Collections;

public class clicker : MonoBehaviour {

	private int points = 0;
	private int eachClickWorth = 1;
	public GUIStyle pointsStyle;
	private int totalTaps = 0;
	public int timeInLevel = 1; // in seconds
	private string circleString = "Tap to gain points"; 


	// TIMER
	private float timer = 0;
	private bool isTiming = false;
	private bool isFinished = false;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.GetInt("gamesPlayed") + 1);
		pointsStyle.alignment = TextAnchor.MiddleCenter;
	}

	void Update(){
		if(isTiming){
			timer += Time.deltaTime;
			circleString = ((timeInLevel - timer)).ToString("#.##");
		}

		// Time is over
		if (timer > timeInLevel && !isFinished)
		{
			Debug.Log("Game is over");
			GameObject.Find("end").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("end").GetComponent<BoxCollider2D>().enabled = true;
			isFinished = true;
		}
	}

	public void end(){
		PlayerPrefs.SetInt("totalTaps", PlayerPrefs.GetInt("totalTaps") + totalTaps);
		PlayerPrefs.SetInt("totalTimePlayed", PlayerPrefs.GetInt("totalTimePlayed") + timeInLevel);

		if(timeInLevel == 10){
			if(PlayerPrefs.GetInt("10sHS") < points){
				PlayerPrefs.SetInt("10sHS", points);
			}
		}

		if(timeInLevel == 30){
			if(PlayerPrefs.GetInt("30sHS") < points){
				PlayerPrefs.SetInt("30sHS", points);
			}
		}

		Application.LoadLevel("home");
	}

	void OnMouseDown() {
		if(!isFinished){
			if(points == 0){
				// Then we start the timer
				isTiming = true;
			}
			this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		}
	}

	void OnMouseUp() {
		if(!isFinished){
			this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
			totalTaps++;
			points += eachClickWorth;
		}
	}

	void OnGUI() {
		if(!isFinished){
			pointsStyle.fontSize = Screen.height / 25;
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 3.25f, 200, 20), points.ToString(), pointsStyle);

			if(isTiming){
				pointsStyle.fontSize = Screen.height / 10;
			}
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - Screen.height / 2.75f, 200, 20), circleString.ToString(), pointsStyle);
		}else{
			pointsStyle.fontSize = Screen.height / 15;
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 3.25f, 200, 20), points.ToString(), pointsStyle);

			pointsStyle.fontSize = Screen.height / 15;
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - Screen.height / 2.75f, 200, 20), "Score: " + points.ToString(), pointsStyle);
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - Screen.height / 4.5f, 200, 20), "Back", pointsStyle);
		}
	}
}
