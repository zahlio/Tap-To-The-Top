using UnityEngine;
using System.Collections;

public class reaction : MonoBehaviour {

	public string circleString = "Tap to start";
	public GUIStyle pointsStyle;
	public bool startedGame = false;

	public float timer = 0;
	public float nextStart = 0;

	public bool mini = false; 
	public float timer2 = 0;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.GetInt("gamesPlayed") + 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(startedGame){
			timer += Time.deltaTime;

			// When we time the reaction
			if(mini){
				timer2 += Time.deltaTime;
			}

			// We see if we should start the reaction game
			if(timer >= nextStart && !mini){
				timer2 = 0;
				mini = true;
				this.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	void OnMouseDown() {
	}
	
	void OnMouseUp() {
		if(!startedGame){
			startedGame = true;
			circleString = "";
			nextStart = Random.Range(2.0f, 4.0F) + timer;
		}

		if(mini){
			this.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			nextStart = Random.Range(2.0f, 4.0F) + timer;
			circleString = timer2.ToString();
			mini = false;

			// Set new highscore
			if(PlayerPrefs.GetFloat("reactionTimeBest") > timer2 || PlayerPrefs.GetFloat("reactionTimeBest") == 0){
				PlayerPrefs.SetFloat("reactionTimeBest", timer2);
				circleString = "New HS! \n" + timer2.ToString();
			}
		}
	}

	void OnGUI() {
		pointsStyle.fontSize = Screen.height / 30;
		GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 3.25f, 200, 20), "Tap when the circle changes size", pointsStyle);

		pointsStyle.fontSize = Screen.height / 20;
		GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - Screen.height / 2.75f, 200, 20), circleString.ToString(), pointsStyle);
	}
}
