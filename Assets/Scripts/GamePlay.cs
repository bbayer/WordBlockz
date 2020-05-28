using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GamePlay : MonoBehaviour {

	public Transform wordPrefab;
	public Transform letterPrefab;
	public GameObject cursor;
	public Level levelData;
	public Text levelText;
	public Transform letterRoot;
	public List<SpriteLetter> selectedLetterList;
	private List<Word> words ;
	public LineRenderer lineRenderer;
	private List<string> foundWords;
	public TextMesh attemptedWord;
	public LevelCompletedUI levelCompletedUI;
	public ParticleSystem levelEndParticules;
	int nextHintedStartIndex;
	bool isGamePaused;
	// Use this for initialization
	void Start () {
//		if (LevelManager.Instance == null) {
//			//levelData = new LevelManager.Level ("Starter Blockz", "NSO", new string[]{ "SOOON", "ONS", "NO","SOOON","SOOON","SOOON","SOOOsssN"}, 0);
//			//levelData = new LevelManager.Level ("Starter Blockz", "NSO", new string[]{ "SOOON", "ONS", "NO","SOOON","SOOON","SOOON","SOOON","SOOOON"}, 0);
//			levelData = new LevelManager.Level ("Starter Blockz", "NSO", new string[]{ "SOOON", "ONS", "NO","SOON","SOON",}, 0);
//
//		} else {
//			levelData = LevelManager.Instance.levels [GameManager.Instance.currentSelectedLevel];
//		}
		levelData = LevelLoader.Instance.levels[GameManager.Instance.currentSelectedLevel];
		words = new List<Word> ();
		//MakeWordPlacements2 ();
		CreateAndAlignWords();
		levelText.text = levelData.name;
		MakeLetterPlacements();
		selectedLetterList = new List<SpriteLetter>();
		foundWords = new List<string>();
		//lineRenderer = GetComponent<LineRenderer>();
		cursor.SetActive (false);
		levelCompletedUI.gameObject.SetActive (false);
		levelEndParticules.gameObject.SetActive (false);
		isGamePaused=false;

	}

	void OnEnable(){		
		GameManager.Instance.OnGameEvent += OnGameEvent;
	}
	void OnDisable(){
		GameManager.Instance.OnGameEvent -= OnGameEvent;

	}

	void MakeLetterPlacements(){
		float angleDiff = 2*Mathf.PI/levelData.letters.Length;
		float radius=2.3f;
		for(int i=0;i<levelData.letters.Length;i++){
			
			Transform tr = Instantiate (letterPrefab, letterRoot);
			SpriteLetter lttr = tr.GetComponent<SpriteLetter> ();
			lttr.letter = levelData.letters[i];
			lttr.color = GameManager.Instance.colors[i % GameManager.Instance.colors.Count];
			tr.localPosition = new Vector3(Mathf.Sin(angleDiff*i)*radius, Mathf.Cos(angleDiff*i)*radius,2);
			tr.localScale = new Vector2(1.2f, 1.2f);
		}
	}
//	void MakeWordPlacements2(){
//		float rowHeight = 0;
//
//		foreach (string word in levelData.words) {
//			Color c = GameManager.Instance.colors [transform.childCount % GameManager.Instance.colors.Count];
//			Transform tr = Instantiate (wordPrefab, transform);
//			Word wrd = tr.GetComponent<Word> ();
//			wrd.color = c;
//			wrd.SetWord (word);
//			wrd.SetDisable (true);
//			//tr.position = new Vector2(0, rowHeight);
//			CenterWord(wrd,rowHeight);
//			rowHeight+=1.22f;
//		}
//		//float width = Camera.main.orthographicSize*(float)Screen.width / (float)Screen.height;
//		float width = 4.5f;
//		//Camera.main.orthographicSize = width * (float)Screen.height / (float)Screen.width;
//		//words = GetComponentsInChildren<Word> ();
//
//		if(levelData.words.Length>5)
//		{
//			int totalLength=0;
//			GameObject group = new GameObject ("group 1");
//			group.transform.SetParent (transform);
//			rowHeight = 0;
//			for (int i = 0; i < words.Count; i++) {
//				totalLength += levelData.words [i].Length;
//				if (totalLength >= 8) {
//					CenterGroup (group.transform);
//					group = new GameObject ("group 1");
//					group.transform.SetParent (transform);
//					totalLength = levelData.words [i].Length;
//					rowHeight += 1.22f;
//					group.transform.position =new Vector2(0, rowHeight);
//
//				}
//				words [i].transform.parent = group.transform;
//				if (group.transform.childCount == 1) {
//					words [i].transform.localPosition = new Vector2 (0,0);
//				} else {
//					words [i].transform.localPosition = new Vector2 ((totalLength - levelData.words [i].Length) *1.05f+0.5f,0);
//				}
//			}
//			CenterGroup(group.transform);
//		}
//		transform.position = new Vector2(0,(1.22f*5 - rowHeight)/2);
//		foreach(Word wrd in words){
//			wrd.InitAnimation();
//		}
//	}

	void CreateAndAlignWords(){
		float height = Camera.main.orthographicSize;
		float width = (float)Screen.width/Screen.height  * height;
		foreach (string word in levelData.words) {
			Color c = GameManager.Instance.colors [transform.childCount % GameManager.Instance.colors.Count];
			Transform tr = Instantiate (wordPrefab, transform);
			Word wrd = tr.GetComponent<Word> ();
			wrd.color = c;
			wrd.SetWord (word);
			wrd.SetDisable (true);
			words.Add (wrd);
		}

		if (levelData.words.Length > 5) {
			int groupLength = 0;
			GameObject group = new GameObject ("group");
			group.transform.SetParent (transform);
			for (int i = 0; i<words.Count; i++) {
				Word selectedWord = words [i];
				if (groupLength + selectedWord.word.Length >8) {					
					AlignGroup (group.transform);
					group = new GameObject ("group");
					group.transform.SetParent (transform);
					groupLength = 0;

				}
				selectedWord.transform.SetParent (group.transform);
				groupLength+=selectedWord.word.Length;
			}
			AlignGroup (group.transform);
		}
		float maxWidth =  AlignWords ();
		//Align gameplay

		float offset = -0.3f;
		if (maxWidth > width*2) {
			transform.localScale = new Vector3 (.8f, .8f, 1);
			offset = .3f;
		}
		transform.position = new Vector2 (0,height/2f-transform.childCount*0.61f);
	}

	void AlignGroup(Transform groupTransform){
		float childWidth = 0;
		for (int i = 0; i < groupTransform.childCount; i++) {
			Word w = groupTransform.GetChild (i).GetComponent<Word> ();
			w.transform.position = new Vector2 (i * 0.35f + childWidth, 0);
			childWidth += w.width;
		}
	}

	float AlignWords(){
		float height = 0;
		float verticalSpace = 1.22f;
		float maxTotalWidth = 0;
		foreach (Transform t in transform) {
			Word[] words = t.GetComponentsInChildren<Word> ();
			float totalWidth = 0;
			foreach (Word w  in words)
				totalWidth += w.width;
			totalWidth += (words.Length - 1)*0.35f;
			maxTotalWidth = Mathf.Max (maxTotalWidth, totalWidth);
			t.position = new Vector2 (-totalWidth / 2, height);
			height += verticalSpace;
		}
		return maxTotalWidth;
	}

	void CenterWord(Word w, float height){
		w.transform.position = new Vector2 (-w.width/2,height);
	}

	void CenterGroup(Transform t){
		float totalWidth = 0;
		foreach (Transform child in t) {
			Word w = child.GetComponent<Word> ();
			totalWidth += w.width;
		}
		totalWidth += (t.childCount-1) * 0.5f;
		t.position = new Vector2 (-totalWidth / 2, t.position.y);
	}

	// Update is called once per frame
	bool selectMode=false;
	void Update () {
		if(isGamePaused)
			return;
		if(Input.GetMouseButtonDown(0)){
			selectMode=true;
			selectedLetterList.Clear();
			lineRenderer.positionCount=1;
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			cursor.transform.position = pos;
			foreach (var letter in letterRoot.GetComponentsInChildren<SpriteLetter>()) {
				letter.Highlight (false);
			}
			cursor.SetActive (true);
		}
		else if(Input.GetMouseButtonUp(0)){
			selectMode=false;
			lineRenderer.positionCount=1;

			foreach(SpriteLetter l in selectedLetterList){
				l.Highlight(false);
			}
			Word w = CheckIfMatch();
			if (w != null) {
				//Play animation
				w.PlayRevealAnimation ();
				CheckIfLevelEnds ();
			}
			cursor.SetActive (false);

			foreach (var letter in letterRoot.GetComponentsInChildren<SpriteLetter>()) {
				letter.Highlight (false);
			}
			attemptedWord.text = "";
		}

		if(selectMode){
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			cursor.transform.position = pos;
			int lastNdx = lineRenderer.positionCount -1;
			lineRenderer.SetPosition(lastNdx,pos);
		}
	}

	public void  OnLetterSelected(Collider2D lttr){
		Debug.Log("collider hit");
		SpriteLetter sl = lttr.gameObject.GetComponent<SpriteLetter>();
		if(selectedLetterList.IndexOf(sl)<0){
			selectedLetterList.Add(sl);
			Vector3 pos = new Vector3(sl.gameObject.transform.position.x,sl.gameObject.transform.position.y,-3);
			lineRenderer.SetPosition(lineRenderer.positionCount-1,pos);
			lineRenderer.positionCount+=1;
			if(lineRenderer.positionCount ==2){
				lineRenderer.startColor = sl.color;
				lineRenderer.endColor = sl.color;
				attemptedWord.color = sl.color;

			}
			SoundManager.Instance.Play ("letter selected");
		}
		sl.Highlight(true);
		attemptedWord.text = "";
		foreach(SpriteLetter s in selectedLetterList){
			attemptedWord.text+=s.letter;
		}
	}

	public Word CheckIfMatch(){
		string word="";
		foreach(SpriteLetter s in selectedLetterList){
			word+=s.letter;
		}

		Debug.Log("Checking if match:"+word);
		Predicate<string> strPre = (string p)=>{return p.Equals(word);};
//		Debug.Log(Array.Find(levelData.words,strPre));
//		Debug.Log(foundWords.Find(strPre));
		if (foundWords.Find (strPre) != null) {
			int ndx = Array.FindIndex (levelData.words, strPre);
			words[ndx].AlreadyFound ();
		} else if (Array.Find (levelData.words, strPre) != null) {
			foundWords.Add (word);
			int ndx = Array.FindIndex (levelData.words, strPre);
			Debug.Log ("Found a match:" + ndx);
			//SoundManager.Instance.Play ("found match");
			return words [ndx];
		} else {
			if(word.Length>1)
				GameManager.Instance.coinCount -= 5;
		}
		return null;
	}

	public void CheckIfLevelEnds(){
		if(foundWords.Count == levelData.words.Length){
			Debug.Log("Level finished");
			//LevelManager.Instance.CompleteLevel(levelData.index);
			LevelLoader.Instance.CompleteLevel(levelData.index);
			StartCoroutine(PlayLevelEndAnimation());
		}
	}

	public IEnumerator PlayLevelEndAnimation(){
		Debug.Log("Playin Level End animation");
		yield return new WaitForSeconds (.4f);
		LeanTween.scale (letterRoot.gameObject, new Vector3 (0, 0, 0), 0.4f).setEaseOutBounce ();
//		foreach (Word w in words) {
//			w.PlayRevealAnimation ();
//			yield return new WaitForSeconds (0.5f);
//		}
		//yield return new WaitForSeconds (.3f);
		levelCompletedUI.gameObject.SetActive (true);
		levelEndParticules.gameObject.SetActive (true);
		SoundManager.Instance.Play ("level completed");
	}

	public void OnHintClicked(){
		GameManager.Instance.PublishEvent ("hint_clicked",null);
		if (GameManager.Instance.coinCount < 25) {
			GameManager.Instance.PublishEvent ("activate_shop_ui",null);

			return;
		}
		for (int i = nextHintedStartIndex; i < words.Count; i++) {
			if (!words [i].isSolved) {
				bool retval = words [i].ShowHint ();
				if (retval) {
					GameManager.Instance.coinCount -= 25;
					nextHintedStartIndex = (nextHintedStartIndex + 1 )% words.Count;
					return;
				}
			}
		}

		for (int i = 0; i < nextHintedStartIndex; i++) {
			if (!words [i].isSolved) {
				bool retval = words [i].ShowHint ();
				if (retval) {
					GameManager.Instance.coinCount -= 25;
					nextHintedStartIndex = (nextHintedStartIndex + 1 )% words.Count;
					return;
				}
			}
		}
	}

	void OnGameEvent(string name, object param){
		if (name == "shop ui active status") {
			bool status = (bool)param;
			isGamePaused = status;
		} else if (name == "new challenge unlocked") {
			levelCompletedUI.NewChallengeUnlocked ();
		}
			
	}
		
}
