using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {
	int totalCoins;
	int startNdx;
	int endNdx;
	public override void  OnInspectorGUI () {
		//Called whenever the inspector is drawn for this object.
		DrawDefaultInspector();

		EditorGUILayout.BeginHorizontal(GUIStyle.none);
		totalCoins = EditorGUILayout.IntSlider(totalCoins,0,500);
		if(GUILayout.Button("set coin")){
			PlayerPrefs.SetInt("coinCount",totalCoins);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(GUIStyle.none);
		startNdx=EditorGUILayout.IntField ("start index",startNdx,GUIStyle.none,null);
		endNdx=EditorGUILayout.IntField ("end index",endNdx,GUIStyle.none,null);
		if(GUILayout.Button("lock specific")){
			for (int i = startNdx; i < endNdx; i++) {
				LockLevel (i);
			}
			PlayerPrefs.SetInt (string.Format("Level {0}_isUnlocked",endNdx), 1);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(GUIStyle.none);
		startNdx=EditorGUILayout.IntField ("start index",startNdx,GUIStyle.none,null);
		endNdx=EditorGUILayout.IntField ("end index",endNdx,GUIStyle.none,null);
		if(GUILayout.Button("unlock specific")){
			for (int i = startNdx; i < endNdx; i++) {
				UnlockLevel (i);
			}
		}
		EditorGUILayout.EndVertical();

		//This draws the default screen.  You don't need this if you want
		//to start from scratch, but I use this when I'm just adding a button or
		//some small addition and don't feel like recreating the whole inspector.
		if(GUILayout.Button("clear playerprefs")) {
			//add everthing the button would do.
			PlayerPrefs.DeleteAll();
		}
		else if(GUILayout.Button("unlock all")) {
			//add everthing the button would do.
			for(int i=0;i<300;i++){
				UnlockLevel (i);
			}
		}
		else if(GUILayout.Button("lock all")) {

			for(int i=0;i<300;i++){
				LockLevel (i);
			}
			var textAssets = Resources.LoadAll("Levels",typeof(TextAsset));
			foreach (TextAsset t in textAssets) {
				string name = t+"_isUnlocked";
				PlayerPrefs.SetInt (name + "_isUnlocked", 0);
			}
		}

	}

	public void LockLevel(int ndx){
		string name = "Level " + ndx.ToString();
		PlayerPrefs.SetInt (name + "_isSolved", 0);
		PlayerPrefs.SetInt (name + "_isUnlocked", 0);
	}

	public void UnlockLevel(int ndx){
		string name = "Level " + ndx.ToString();
		PlayerPrefs.SetInt (name + "_isSolved",  1);
		PlayerPrefs.SetInt (name + "_isUnlocked",  1);
	}
}

