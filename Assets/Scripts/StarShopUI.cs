using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarShopUI : MonoBehaviour {

	public Text starCountText;
	public RectTransform mainPanel;
	public RectTransform btnRectTransform;
	// Use this for initialization
	void Start () {
		mainPanel.localScale = new Vector3 (0,0,0);
		LeanTween.scale (mainPanel, new Vector3 (1, 1, 1), .5f).setEaseOutBack ();//setEaseOutBounce();
		starCountText.text = "You have "+GameManager.Instance.coinCount+"\nstars remaining.";
		GameManager.Instance.PublishEvent("shop ui active status",true);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCloseClicked(){
		LeanTween.scale (mainPanel, new Vector3 (0, 0, 0), .3f).setEaseInBack ().setOnComplete (CloseAnimationCompleted);
		SoundManager.Instance.Play ("close");
		GameManager.Instance.PublishEvent("shop ui active status", false);
	}

	void CloseAnimationCompleted(){
		Destroy (gameObject);
	}

	public void OnEarnStarClicked(){
		Debug.Log ("earn video clicked");
		//GameManager.Instance.coinCount += 25;
		GameManager.Instance.GiveRewardWithAnimation(btnRectTransform, 25);
			
	}
}
