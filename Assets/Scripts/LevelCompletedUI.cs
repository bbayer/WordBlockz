using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedUI : MonoBehaviour {

	public Text levelCompletedText;

	public RectTransform mainPanel;
	public RectTransform okImage;
	// Use this for initialization
	void Awake(){
	}

	void Start () {
		mainPanel.localScale = new Vector3 (0, 0, 0);
		okImage.localScale = new Vector3 (0, 0, 0);
		LeanTween.scale (mainPanel, new Vector3 (1, 1, 1), .5f).setEaseOutBack();
		LeanTween.scale (okImage, new Vector3 (1, 1, 1), 0.5f).setEaseOutBounce().setDelay (.4f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnNextButtonClicked(){
		GameManager.Instance.PlayNextLevel ();
		SoundManager.Instance.Play ("click");

	}

	public void OnLevelsButtonClicked(){
		GameManager.Instance.Levels();
		SoundManager.Instance.Play ("click");

	}

	public void OnShareButtonClicked(){
		GameManager.Instance.Share ();
		SoundManager.Instance.Play ("click");

	}

	public void NewChallengeUnlocked(){
		levelCompletedText.text="YAY!\nNEW CHALLENGE\nUNLOCKED!";
	}
}
