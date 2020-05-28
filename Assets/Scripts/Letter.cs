using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Letter : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	private char _letter;
	public char letter{
		get{ 
			return _letter;
		}

		set{
			gameObject.name = value.ToString();
			_letter = value;
			GetComponentInChildren<Text> ().text = _letter.ToString();
		}
	}
		
	public bool noShadow;
	public Sprite noShadowSprite;

	void Awake(){
	}

	void Start () {
		letter = _letter;
		if (noShadow) {
			Image img = GetComponent<Image> ();
			img.sprite = noShadowSprite;
			img.type = Image.Type.Simple;
		}

	}


	// Update is called once per frame
	void Update () {
		
	}

	public void AssignRandomColor(){
		int ndx = Random.Range (0, GameManager.Instance.colors.Count);
		Debug.Log (ndx);
		SetColor (GameManager.Instance.colors [ndx]);
	}

	public void SetColor(Color color){
		GetComponent<Image> ().color = color;
	}

	public void SetColorIndex(int ndx){
		ndx = ndx % GameManager.Instance.colors.Count;
		SetColor(GameManager.Instance.colors[ndx]);
	}
}
