using UnityEngine;
using System.Collections;


public class Advertiser : MonoBehaviour {

	public string pubID;

	void OnEnable(){
		ScoreKeeper.OnStartGame += OnGameStart;
	}

	void OnDisable(){
		ScoreKeeper.OnStartGame -= OnGameStart;
	}

	void Start () {
		HeyzapAds.start(pubID, HeyzapAds.FLAG_NO_OPTIONS);
	}


	void OnGameStart(){
		HZInterstitialAd.show();
	}
}
