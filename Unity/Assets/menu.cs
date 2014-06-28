using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	public string text = "NONE";
	public string levelToLoad = "NONE";
	public int xVal = 50;
	public GUIStyle textStyle;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, 0);
		
		textStyle.fontSize = 40;
		GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height / 100) * xVal + 10, 200, 20), text, textStyle);
	}

	void OnMouseDown() {
		this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	}
	
	void OnMouseUp() {
		this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
		Application.LoadLevel(levelToLoad);
	}
}
