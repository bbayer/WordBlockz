using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelList : MonoBehaviour {

	public LevelButton levelButtonPrefab;
//	public LevelCategoryTitle levelCategoryPrefab;
	public LevelChallengeButton levelChallengeButton;
	public Transform scrollContentArea;
	public Text titleText;
	private enum ListMode
	{
		CHALLENGE,
		LEVELS
	}
	private ListMode listMode;
	// Use this for initialization
	void Start () {
//		string lastCategory="";
//		foreach (LevelManager.Level level in LevelManager.Instance.levels) {
//			if (level.category != lastCategory) {
//				AddCategory (level.category);
//			}
//			AddLevelButton (level);
//			lastCategory = level.category;
//		}
		ListChallanges();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ClearList(){
		foreach(Transform t in scrollContentArea){
			Destroy(t.gameObject);
		}
	}

	public void ListChallanges(){
		ClearList();
		foreach (Challenge ch in LevelLoader.Instance.challenges) {
			if(ch.totalCount!=0)
				AddChallenge (ch);
		}
		listMode = ListMode.CHALLENGE;
		titleText.text = "CHALLENGES";
	}

	void OnEnable(){		
		GameManager.Instance.OnGameEvent += OnGameEvent;
	}
	void OnDisable(){
		GameManager.Instance.OnGameEvent -= OnGameEvent;

	}
	void OnGameEvent(string name, object value){
		if (name == "challenge selected") {
			ListLevels((Challenge) value);
		}
	}

	public void ListLevels(Challenge challenge){
		ClearList();

		for(int i=challenge.startIndex;i<challenge.startIndex+challenge.totalCount;i++){
			Level lvl = LevelLoader.Instance.levels[i];
			AddLevelButton(lvl);
		}

		listMode = ListMode.LEVELS;
		titleText.text = challenge.name.ToUpper();
	}

//	void AddCategory(string title){
//		LevelCategoryTitle levelCategoryTitle = Instantiate (levelCategoryPrefab, scrollContentArea) as LevelCategoryTitle;
//		levelCategoryTitle.SetTitle (title);
//	}

	void AddChallenge(Challenge challenge){
		LevelChallengeButton challengeButton = Instantiate (levelChallengeButton, scrollContentArea) as LevelChallengeButton;
		challengeButton.SetChallenge(challenge);
	}

//	void AddLevelButton(LevelManager.Level level){
	void AddLevelButton(Level level){
		LevelButton levelButton = Instantiate (levelButtonPrefab, scrollContentArea) as LevelButton;
		levelButton.levelTitle.text = level.name;
		levelButton.description.text = level.maxWordLength.ToString () + " blockz at most";
		levelButton.levelIndex = level.index;
		levelButton.SetLetters (level.letters);
		levelButton.SetUnlock (level.isUnlocked, level.isSolved);
	}

	public void OnBackClicked(){
		if (listMode == ListMode.CHALLENGE) {
			GameManager.Instance.MainMenu ();
		} else if (listMode == ListMode.LEVELS) {
			ListChallanges ();
		}
		SoundManager.Instance.Play("click");
	}
}
