using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DailyAwardButton : MonoBehaviour {

	public GameObject menuPanelPrefab;
	public Image exclamationIcon;
	// Use this for initialization
	void Awake(){
		exclamationIcon.gameObject.SetActive (false);
	}
	void Start () {
		
	}

	void OnEnable(){		
		GameManager.Instance.OnGameEvent += OnGameEvent;
	}

	void OnDisable(){
		GameManager.Instance.OnGameEvent -= OnGameEvent;

	}

	void OnGameEvent(string name, object value){
		Debug.Log ("reward button event handler");
		if (name == "new reward unlocked") {			
			exclamationIcon.gameObject.SetActive (true);
		} else if (name == "reward claimed") {
			exclamationIcon.gameObject.SetActive (false);
		} else if (name == "unclaimed reward found") {
			exclamationIcon.gameObject.SetActive (true);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonClicked(){
		Instantiate (menuPanelPrefab, transform.parent);
		SoundManager.Instance.Play ("click");
	}

	public void AnimateButton(){
		RectTransform rect = GetComponent<RectTransform> ();
		LeanTween.cancel (rect);
		rect.localScale = Vector3.one;
		LeanTween.scale (rect, new Vector3 (1.2f, 1.2f, 1.2f), .3f).setEaseInBack ().setLoopPingPong (1);
	}
}
