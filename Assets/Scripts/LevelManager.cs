using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : MonoBehaviour {


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
		public string category;
		public int maxWordLength;
		public Level(string category, string letters, string[] words, int ndx, string name=""){
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
			this.category=category;
			maxWordLength=0;
			foreach(string str in words){
				maxWordLength=Math.Max(str.Length,maxWordLength);
			}
		}
	}
		
	public List<Level> levels;

	public static LevelManager Instance;
	void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (this);
			return;
		}

		levels = new List<Level> ();
		Level first = new Level ("Starter Blockz", "DDA", new string[]{"DAD","ADD" }, levels.Count);
		first.isUnlocked = true;
		levels.Add (first);
		levels.Add (new Level ("Starter Blockz","TAC",new string[]{"ACT","CAT"}, levels.Count));
		levels.Add (new Level ("Starter Blockz","ABR",new string[]{"BAR","BRA"}, levels.Count));
		levels.Add (new Level ("Starter Blockz","AEP",new string[]{"APE","PEA"}, levels.Count));
		levels.Add (new Level ("Starter Blockz","TRA",new string[]{"ART","RAT","TAR"}, levels.Count));
		levels.Add (new Level ("Starter Blockz","NSO",new string[]{"SON","ONS","NOS"}, levels.Count));

		levels.Add (new Level ("Beginner Blockz", "NSO", new string[]{ "SOOON", "ONS", "NO", "SOON", "SOON", "SOON"}, levels.Count));
		levels.Add (new Level ("Beginner Blockz","ABCDEFGH",new string[]{}, levels.Count));
		levels.Add (new Level ("Beginner Blockz","ABCDE",new string[]{}, levels.Count));
		levels.Add (new Level ("Beginner Blockz","ABCDE",new string[]{}, levels.Count));
		levels.Add (new Level ("Beginner Blockz","ABCDE",new string[]{}, levels.Count));
		levels.Add (new Level ("Beginner Blockz","ABCDE",new string[]{}, levels.Count));


	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CompleteLevel(int ndx){
		levels [ndx].isSolved = true;
		levels [Math.Min (ndx + 1, levels.Count - 1)].isUnlocked = true;
	}
}
