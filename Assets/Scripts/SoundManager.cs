using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Use this for initialization
	[System.Serializable]
	public struct LabeledClip{
		public string label;
		public AudioClip clip;
	}

	public static SoundManager Instance;

	[SerializeField]
	public List<LabeledClip> clips;
	private AudioSource audioSource;

	void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (this);

	}
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Play(string tag){
		foreach(LabeledClip l in clips){
			if(l.label==tag){
				audioSource.clip = l.clip;
				audioSource.Play();
				return true;
			}
		}
		return false;
	}
}
