using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class Level{
	public string letters;
	public string [] words;

	public bool isSolved{
		get	{ 
			return Convert.ToBoolean(PlayerPrefs.GetInt(name+"_isSolved",0));
		}

		set{

			PlayerPrefs.SetInt(name+"_isSolved",Convert.ToInt32(value));
			if(value){
				this.challenge.updateCompletionRate();
			}
		}
	}

	public bool isUnlocked{
		get	{ 
			return Convert.ToBoolean(PlayerPrefs.GetInt(name+"_isUnlocked",0));
		}

		set{
			PlayerPrefs.SetInt(name+"_isUnlocked",Convert.ToInt32(value));
		}
	}

	public int index;
	public string name;
	public Challenge challenge;
	public int maxWordLength;

	public Level(Challenge challenge, string letters, string[] words, int ndx, string name=""){
		this.letters = letters;
		Array.Sort(words, (x,y)=> x.Length-y.Length);


		this.words = words;
		if(name!=""){
			this.name = name;
		}
		else{
			this.name = "Level "+(ndx+1);	
		}
		this.index=ndx;
		this.challenge=challenge;
		maxWordLength=0;
		foreach(string str in words){
			maxWordLength=Math.Max(str.Length,maxWordLength);
		}
		//Debug.Log(words.Length);
	}

	public static Level fromString(Challenge challenge, int ndx, string str){
		if(str.Length==0)
			return null;
		string trimmed=str.Trim ();
		string[] tmp = trimmed.Split(':');
		string letters = tmp[0];
		string[] words = tmp[1].Split(',');
		return new Level(challenge, letters, words, ndx);
	}
}

public class Challenge{
	public string name;
	public int startIndex;
	public int totalCount;
	public int completedCount;

	public bool isUnlocked{
		get	{
			bool val = false;
			for (int i = startIndex; i < startIndex + totalCount; i++) {
				val |= LevelLoader.Instance.levels [i].isUnlocked;
			}
			return val;
			//return Convert.ToBoolean(PlayerPrefs.GetInt(name+"_isUnlocked",0));
		}

//		set{
//			PlayerPrefs.SetInt(name+"_isUnlocked",Convert.ToInt32(value));
//		}
	}

	public int completionRate{
		get{
			return PlayerPrefs.GetInt("CompletionRateOf_" + name,0);
		}
	}

	public Challenge(string name){
		this.name=name;
	}

	public bool isCompleted{
		get{
			return completionRate==100;
		}
	}

	public void updateCompletionRate(){
		completedCount=0;
		if (LevelLoader.Instance.levels.Count < startIndex || totalCount == 0)
			return;
		for(int i=startIndex;i<startIndex+totalCount;i++){
			if(LevelLoader.Instance.levels[i].isSolved)
				completedCount+=1;
		}
		PlayerPrefs.SetInt("CompletionRateOf_" + name,Convert.ToInt32(((float)completedCount/totalCount) * 100));

	}
}

public class LevelLoader 
{

	private string levelPath;
	public List<Level> levels;
	public List<Challenge> challenges;
	private static LevelLoader singletoneInstance;
	public static LevelLoader Instance{
		get{
			if(singletoneInstance == null){
				singletoneInstance = new LevelLoader("Levels");
			}
			return singletoneInstance;
		}
	}
	private LevelLoader(string levelPath){
		this.levelPath=levelPath;
		levels = new List<Level>();
		challenges = new List<Challenge>();
		parseLevels();
		levels[0].isUnlocked=true;
	}

	private void parseLevels(){
		var textAssets = Resources.LoadAll(levelPath,typeof(TextAsset));
		foreach(TextAsset t in textAssets){
			Challenge ch = new Challenge(t.name);
			ch.startIndex=levels.Count;
			foreach (var levelStr in t.text.Split('\n')) {
				if(levelStr.Length!=0)
					levels.Add( Level.fromString(ch,levels.Count,levelStr) );
			}
			ch.totalCount = levels.Count-ch.startIndex;
			challenges.Add(ch);
			Debug.Log (ch.name+" >start:"+ch.startIndex+" >count: "+ch.totalCount);
		}

	}

	public void UpdateCompletionRates(){
		foreach(Challenge ch in challenges){
			ch.updateCompletionRate();
		}
	}

	public void CompleteLevel(int level){
		Challenge levelsChallenge = levels[level].challenge;
		levels [level].isSolved = true;
		levels [Math.Min (level + 1, levels.Count - 1)].isUnlocked = true;
		bool prevIsCompleted = levelsChallenge.isCompleted;
		levelsChallenge.updateCompletionRate();
		if(levelsChallenge.isCompleted && !prevIsCompleted){
			int chNdx = challenges.IndexOf(levelsChallenge);
			int nextChNdx = Math.Min (chNdx + 1, challenges.Count - 1);
			if(nextChNdx>chNdx){
				GameManager.Instance.PublishEvent("new challenge unlocked", challenges [nextChNdx]);
				SoundManager.Instance.Play("congrats");
			}
		}
	}
}

