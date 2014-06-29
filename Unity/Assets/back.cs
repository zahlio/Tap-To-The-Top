using UnityEngine;
using System.Collections;

public class back : MonoBehaviour {
	public GUIStyle textStyle;
	// Use this for initialization
	void Start () {
		textStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown() {
		this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	}
	
	void OnMouseUp() {
		this.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
		Application.LoadLevel("home");
	}

	void OnGUI(){
		
		textStyle.fontSize = Screen.height / 20;
		GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - Screen.height / 4.5f, 200, 20), "Back", textStyle);
	}
}
