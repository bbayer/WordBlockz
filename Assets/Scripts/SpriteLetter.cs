using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLetter : MonoBehaviour {

	public SpriteRenderer highlightSprite;
	public SpriteRenderer backgroundSprite;
	private Color backgroundColor;
	public ParticleSystem particles;
	public bool isHinted{
		get{ 
			return textMesh.gameObject.activeSelf;
		}
	}
	public char letter{
		get{ 
			if (textMesh != null && textMesh.text.Length > 0) {
				return textMesh.text [0];
			}
			return '_';
		}
		set{ 
			if (textMesh != null)
				textMesh.text = value.ToString ();
		}

	}
	public Color color{
		get{ 
			return backgroundSprite.color;
		}

		set{ 
			backgroundSprite.color = value;
			backgroundColor = color;
			float h,s,v;
			Color.RGBToHSV(color,out h,out s,out v);
			s=Mathf.Min(1,s+0.2f);
			highlightSprite.color = Color.HSVToRGB(h,s,v);
			ParticleSystem.MainModule main= particles.main;
			main.startColor = color;
			//particles.main = main;
		}
	}
	public TextMesh textMesh;
	// Use this for initialization
	void Start () {
		particles.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Highlight(bool val){
		highlightSprite.gameObject.SetActive (val);

	}

	public void SetDisable(bool val){
		if (val) {
			textMesh.gameObject.SetActive (false);
			backgroundSprite.color = new Color (.9f, .9f, .9f, 1);
		} else {
			textMesh.gameObject.SetActive (true);
			color = backgroundColor;
			particles.Play ();
			if(gameObject.activeSelf)
				SoundManager.Instance.Play ("letter popping");
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("trigger enter sprite");
	}

	public void SetHint(bool enable){
		textMesh.gameObject.SetActive (enable);
	}
}
