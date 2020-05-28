using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public Text levelTitle;
	public Text description;
	public Transform lettersPanel;

	public bool isUnlocked;
	public Letter letterPrefab;
	public int levelIndex;
	public Image lockImage;
	public Image solvedImage;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetLetters(string letters){
		int i = 0;
		foreach(char letter in letters){
			Letter letterObj = Instantiate (letterPrefab, lettersPanel) as Letter;
			letterObj.noShadow = true;
			letterObj.letter = letter;
			letterObj.SetColorIndex (i++);
		}
	}
		
	public void OnButtonClicked(){
		Debug.Log ("Button clicked: " + levelIndex);
		if (isUnlocked) {
			SoundManager.Instance.Play ("click");
			GameManager.Instance.PlayLevel (levelIndex);
			
		} else {
			SoundManager.Instance.Play ("error");
		}
	}

	public void SetUnlock(bool isUnlocked, bool isSolved){
		this.isUnlocked = isUnlocked;
		if(isUnlocked){
			lockImage.gameObject.SetActive(false);
		}
		if (!isSolved) {
			solvedImage.gameObject.SetActive(false);
		}
	}
}
	