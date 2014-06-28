using UnityEngine;
using System.Collections;

public class back_icon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
}
