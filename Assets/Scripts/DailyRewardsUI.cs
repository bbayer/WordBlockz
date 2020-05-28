using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DailyRewardsUI : MonoBehaviour {
	public RectTransform mainPanel;
	public List<Button> rewardButtons;
	public Sprite lockSprite;
	public Sprite claimedSprite;
	// Use this for initialization
	void Start () {
		mainPanel.localScale = new Vector3 (0,0,0);
		LeanTween.scale (mainPanel, new Vector3 (1, 1, 1), .5f).setEaseOutBack ();
		SetupDailyRewardButtons ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupDailyRewardButtons(){
		for (int i = 0; i < rewardButtons.Count; i++) {
			Button btn = rewardButtons [i];
			Text[] txts = btn.GetComponentsInChildren<Text> ();
			txts [0].text = string.Format ("DAY {0} - {1} STARS",i+1, GameManager.Instance.rewardCoins[i]);
			bool isUnlocked = Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Unlocked_" + i.ToString (), 0));
			bool isClaimed = Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Claimed_" + i.ToString (), 0));

			if (isUnlocked) {
				if (isClaimed) {
					
					SetStateOfButton (btn, GameManager.Instance.colors [6], claimedSprite, "Claimed.", false);
				} else {
					SetStateOfButton (btn, GameManager.Instance.colors [3], null, "Ready to claim.", true);
				}
			} else {
				SetStateOfButton (btn, new Color(.5f,.5f,.5f), lockSprite, "Not ready yet.", false);
			}
			int tempInt = i;
			btn.onClick.AddListener(()=> {				
				OnButtonClicked(tempInt);
			});
		}
	}

	public void SetStateOfButton(Button btn, Color clr, Sprite spr, string description, bool state){
		Text[] txts = btn.GetComponentsInChildren<Text> ();
		Image[] img = btn.GetComponentsInChildren<Image> ();
		if (spr != null) {
			img [1].sprite = spr;
			img [1].gameObject.SetActive (true);
		} else {
			img [1].gameObject.SetActive (false);
		}
		txts [1].text = description;
		img [0].color = clr;
		btn.enabled = state;

	}

	public void OnButtonClicked(int btn){
		bool isUnlocked = Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Unlocked_" + btn.ToString (), 0));
		bool isClaimed = Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Claimed_" + btn.ToString (), 0));
		if (isUnlocked && !isClaimed) {
			GameManager.Instance.GiveRewardWithAnimation (rewardButtons[btn].GetComponent<RectTransform>(), GameManager.Instance.rewardCoins [btn]);
			PlayerPrefs.SetInt ("Reward_Claimed_" + btn.ToString (), 1);
			GameManager.Instance.PublishEvent ("reward claimed", btn);
		}
	}

	public void OnCloseClicked(){
		LeanTween.scale (mainPanel, new Vector3 (0, 0, 0), .3f).setEaseInBack ().setOnComplete (CloseAnimationCompleted);
		SoundManager.Instance.Play ("close");

	}

	void CloseAnimationCompleted(){
		Destroy (gameObject);
	}
}
