using UnityEngine;
using System.Collections;

public class end : MonoBehaviour {

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
		GameObject.Find("circle").GetComponent<clicker>().end();
	}
}
