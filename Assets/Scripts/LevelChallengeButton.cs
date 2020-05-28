using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChallengeButton : MonoBehaviour {

	public Image playableImage;
	public Image lockedImage;
	public Image solvedImage;
	private Challenge challenge;
	public Text challengeText;
	public Image completionBar;
	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonClick(){
		if (challenge.isUnlocked) {
			GameManager.Instance.PublishEvent ("challenge selected", challenge);
			SoundManager.Instance.Play ("click");
		} else {
			//Make some visual indication.
			RectTransform rect = lockedImage.GetComponent<RectTransform> ();
			LeanTween.cancel (rect);
			rect.localScale = Vector3.one;
			LeanTween.scale (rect, new Vector3 (1.2f, 1.2f, 1.2f), .3f).setEaseInBack ().setLoopPingPong (1);
			SoundManager.Instance.Play ("error");
		}
	}

	public void SetChallenge(Challenge challenge){
		playableImage.gameObject.SetActive(false);
		lockedImage.gameObject.SetActive(false);
		solvedImage.gameObject.SetActive(false);
		this.challenge=challenge;
		if(challenge.isUnlocked){
			playableImage.gameObject.SetActive(true);
		}
		else{
			lockedImage.gameObject.SetActive(true);
		}

		if(challenge.isCompleted){
			playableImage.gameObject.SetActive(false);
			solvedImage.gameObject.SetActive(true);

		}

		challengeText.text = challenge.name+" ("+challenge.completedCount.ToString()+
			"/"+challenge.totalCount.ToString()+")";
		completionBar.rectTransform.localScale = new Vector3(challenge.completionRate/100.0f,1f,1f);
	}
}
