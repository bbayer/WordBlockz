using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI
;
public class CoinButton : MonoBehaviour {

	// Use this for initialization
	public GameObject menuPanelPrefab;
	public Text coinText;

	void Start () {
		coinText.text = GameManager.Instance.coinCount.ToString ();
	}

	void OnEnable(){		
		GameManager.Instance.OnGameEvent += OnGameEvent;
	}
	void OnDisable(){
		GameManager.Instance.OnGameEvent -= OnGameEvent;

	}
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonClicked(){
		Instantiate (menuPanelPrefab, transform.parent.parent);
		SoundManager.Instance.Play ("click");
	}

	void OnGameEvent(string name, object value){
		if (name == "coin_count_updated") {
			coinText.text = GameManager.Instance.coinCount.ToString ();
			RectTransform rect = GetComponent<RectTransform> ();
			LeanTween.cancel (rect);
			rect.localScale = Vector3.one;
			LeanTween.scale (rect, new Vector3 (1.2f, 1.2f, 1.2f), .3f).setEaseInBack ().setLoopPingPong (1);
		}
		else if(name=="activate_shop_ui"){
			OnButtonClicked();
		}
	}
}
