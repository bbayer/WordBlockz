using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NotificationServices=UnityEngine.iOS.NotificationServices;

public class GameManager : MonoBehaviour {


	public delegate void GameEvent(string name, object parameter);
	public event GameEvent OnGameEvent;

	public static GameManager Instance;
	public List<Color> colors;
	public int currentSelectedLevel;
	public int[] rewardCoins;
	public GameObject coinPrefab;
	public int coinCount{
		set{ 
			int count = Mathf.Max (0, value);
			PlayerPrefs.SetInt ("coinCount", count );
			PublishEvent ("coin_count_updated", count);
		}

		get{ 
			return PlayerPrefs.GetInt ("coinCount", -1);
		}
	}

	void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
			OnGameEvent += OnGameEventDebug;
			InitializeValues ();
		} else
			Destroy (this);

	}

	void InitializeValues(){
		if (coinCount == -1) {
			coinCount = 200;
		}
		parameters = new Dictionary<string, object> ();
		LevelLoader.Instance.UpdateCompletionRates();

	}
	// Use this for initialization
	void Start () {
		DailyRewardCalculate ();
		SetupNotification ();
		PlayerPrefs.SetString ("LastPlayDate", System.DateTime.Today.ToString ());

	}
	
	// Update is called once per frame
	void Update () {
		Application.targetFrameRate = 60;
		if (NotificationServices.localNotificationCount > 0) {
			NotificationServices.ClearLocalNotifications ();
		}
	}

	void OnGameEventDebug(string name, object parameter){
		if (parameter == null)
			parameter = "";
		Debug.Log ("OnGameEvent: " + name + " with {" + parameter.ToString () + "}");		
	}

	public void Levels(){
		SceneManager.LoadScene ("levels");
	}

	public void MainMenu(){
		SceneManager.LoadScene ("main");
	}

	public void PlayLevel(int level){
		currentSelectedLevel = level;
		TinySauce.OnGameStarted(levelNumber: level.ToString());
		SceneManager.LoadScene ("play");
	}

	public void PlayNextLevel(){
		currentSelectedLevel = currentSelectedLevel + 1;
		TinySauce.OnGameStarted(levelNumber: currentSelectedLevel.ToString());

		SceneManager.LoadScene ("play");		
	}

	public void CompleteLevel(int level){
		//LevelManager.Instance.CompleteLevel (level);
		TinySauce.OnGameFinished(level.ToString(), 0);
		LevelLoader.Instance.CompleteLevel(level);
	}

	private Color HexToColor( string hex ){
		if (hex.Length == 6)
			hex += "FF";
		byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
		byte a = byte.Parse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber);

		return new Color32 (r, g, b, a);
	}

	public  Color GetRandomColor(){
		return colors[ Random.Range(0,colors.Count)];
	}

	public void PublishEvent(string eventName, object param){
		if(OnGameEvent!=null){
			OnGameEvent(eventName, param);
		}
	}

	public void Share(){
		
	}

	private static Dictionary<string, object> parameters;
	public object GetParameter(string key){		
		if (parameters.ContainsKey(key)) {
			return parameters[key];
		}
		return null;
	}

	public void SetParameter(string key, object value){
		parameters.Add (key, value);
	}

	public void ClearParameters(){
		parameters.Clear ();
	}

	public void DailyRewardCalculate(){
		System.DateTime newDate = System.DateTime.Today;
		string stringDate = PlayerPrefs.GetString("LastPlayDate",newDate.ToString());
		System.DateTime oldDate = System.Convert.ToDateTime(stringDate);
		Debug.Log("LastDay: " + oldDate);
		Debug.Log("CurrDay: " + newDate);

		System.TimeSpan difference = newDate.Subtract(oldDate);
		if(difference.Days >= 1){
			Debug.Log("New Reward!");
			PlayerPrefs.SetString("LastPlayDate", newDate.ToString());
			for (int i = 0; i < rewardCoins.Length; i++) {
				if (PlayerPrefs.GetInt ("Reward_Unlocked_" + i.ToString (), 0) == 0) {
					PlayerPrefs.SetInt ("Reward_Unlocked_" + i.ToString (), 1);
					OnRewardUnlocked (i);
					return;
				}
			}
		}
		CheckForUnclaimedReward ();
	}

	public void SetupNotification(){
		var notification = new UnityEngine.iOS.LocalNotification ();
		notification.fireDate = System.DateTime.Now.AddDays (1);
		notification.alertBody = "Your daily reward is waiting for you!";
		NotificationServices.ScheduleLocalNotification(notification);
	}

	public void OnRewardUnlocked(int ndx){
		PublishEvent ("new reward unlocked", ndx);
	}

	public void CheckForUnclaimedReward(){
		for (int i = 0; i < rewardCoins.Length; i++) {
			bool isClaimed = System.Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Claimed_" + i.ToString (), 0));
			bool isUnlocked = System.Convert.ToBoolean(PlayerPrefs.GetInt ("Reward_Unlocked_" + i.ToString (), 0));
			if (isUnlocked && !isClaimed) {
				PublishEvent ("unclaimed reward found", i);
			}
		}
	}
	public void GiveRewardWithAnimation(RectTransform from, int amount){
//		float dY = Camera.main.orthographicSize - 0.3f;
//		float dX = (float)Screen.width/Screen.height * dY - 0.3f;
//
//		Vector3 rightCorner = new Vector3(dX,dY);
//		Vector3[] pts = new Vector3[]{Vector3.zero,new Vector3(2.5f,0), rightCorner};
//		LTSpline spl=new LTSpline(pts);
//		for(int i=0;i<amount;i++){
//			GameObject coin = Instantiate(coinPrefab,Vector3.zero,Quaternion.identity);
//			LeanTween.move(coin,rightCorner,.5f).setDelay(i*.1f).setOnComplete(item=>{
//				Destroy((GameObject)item);
//				GameManager.Instance.coinCount +=1;
//			},coin);
//		}
		GameManager.Instance.coinCount += amount;
	}

	public void GiveReward(int amount){
		
	}
}
