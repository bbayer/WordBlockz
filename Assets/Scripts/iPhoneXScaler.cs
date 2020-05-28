using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class iPhoneXScaler : MonoBehaviour {
	public float scaleFactor;
	void Awake(){
		Debug.Log ("Screen.width:"+Screen.width);
		Debug.Log ("Screen.height:"+Screen.height);
		CanvasScaler cs = GetComponent<CanvasScaler> ();
		if (cs != null) {
			if (Screen.width == 1135 && Screen.height == 2436) {
				cs.matchWidthOrHeight = scaleFactor;
			}
		}
	}

}
