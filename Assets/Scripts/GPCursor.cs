using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPCursor : MonoBehaviour {

	public GamePlay gameplay;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		gameplay.OnLetterSelected(other);
		Debug.Log("cursor trigger enter");
	}

	void OnTriggerExit2D(Collider2D other){
		gameplay.OnLetterSelected(other);
		Debug.Log("cursor trigger exit");

	}
		
}
