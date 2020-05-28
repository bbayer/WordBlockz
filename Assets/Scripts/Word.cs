using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour {

	public string word;
	public float width;
	public SpriteLetter[] letters;
	public bool isSolved;
	public Color color{
		set{ 
			for (int i = 0; i < letters.Length; i++) {				
				letters [i].color = value;
			}
		}
	}
	// Use this for initialization
	void Awake () {
		letters = GetComponentsInChildren<SpriteLetter> ();
		SetDisable (false);
	}

	public void SetWord(string word){
		this.word = word;
		for (int i = 0; i < word.Length; i++) {
			letters [i].letter = word [i];
		}

		for (int i = word.Length; i < letters.Length; i++) {
			letters [i].gameObject.SetActive (false);
		}
		width = 1.05f * word.Length;
	}
	public void SetDisable(bool disable){
		for (int i = 0; i < word.Length; i++) {
			letters [i].SetDisable (disable);	
		}
		isSolved = !disable;
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void InitAnimation(){
		int count=0;
		float posY=-5f;
		foreach (var letter in letters) {
			if(letter.gameObject.activeSelf){
				Vector3 pos = letter.transform.position;
				letter.transform.position=new Vector3(posY,10f);
				LeanTween.move(letter.gameObject,pos,1f).setEaseOutCubic().setDelay(0.2f*count);
				count+=1;
				posY+=10f/word.Length;
			}
		}
	}

	public void PlayRevealAnimation(float duration=0.1f){
		//TODO:Put some animation
		Debug.Log("Playing reveal anim:"+word);
		StartCoroutine(AnimReveal(duration));
	}

	IEnumerator AnimReveal(float duration=0.1f){
		foreach (SpriteLetter letter in letters) {
			letter.SetDisable (false);
			yield return new WaitForSeconds(duration);
		}
	}

	public bool ShowHint(){
		foreach(SpriteLetter letter in letters){
			if (!letter.isHinted) {
				letter.SetHint (true);
				return true;
			}
		}
		return false;
	}

	public void AlreadyFound(){
		LeanTween.scale (gameObject, new Vector3 (1.1f, 1.1f, 1.1f), 0.3f).setEaseInBounce ().setLoopPingPong (1);
	}
}
