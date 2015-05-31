using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ScoreKeeper : MonoBehaviour {


	public delegate void StartGame();
	public static event StartGame OnStartGame;

	public delegate void GameOvered();
	public static event GameOvered OnGameOver;
	

	public Text scoreText;

	public Text timeText;

	public Text bulletText;

	public Text gameOverText;

	public Text startPannelHighScore;

	public Text newHighScore;

	public GameObject playPannel;

	public GameObject gameOverPannel;

	public GameObject pausePannel;


	public AudioSource musicTrack;

	public AudioSource fxTrack;

	public FXSounds fxSounds;

	public static int score;

	public static int time = 60;

	public static int bullets = 100;

	public static TransitionState transitionState;

	public AudioMixerSnapshot musicMute;

	public AudioMixerSnapshot musicUnMute;


	public bool fxMuted = false;


	public static GameState gameState;


	void OnEnable()
	{
		Duck.OnScore += OnScore;
		Bottle.OnScoreTime += OnScoreTime;
		Can.OnScoreBullets += OnScoreBullets;
		Controls.OnShotBullet += OnShotBullet;
		Controls.OnBulletHit += OnBulletHit;
		CamMove.OnCamUp += OnCamUp;
		CamMove.OnCamDown += OnCamDown;
	}
	
	
	void OnDisable()
	{
		Duck.OnScore -= OnScore;
		Bottle.OnScoreTime -= OnScoreTime;
		Can.OnScoreBullets -= OnScoreBullets;
		Controls.OnShotBullet -= OnShotBullet;
		Controls.OnBulletHit -= OnBulletHit;
		CamMove.OnCamUp += OnCamUp;
		CamMove.OnCamDown += OnCamDown;
	}

	void Start(){



		if(PlayerPrefs.HasKey("HighScore") == false){
			PlayerPrefs.SetInt("HighScore",0);
		}
		startPannelHighScore.text = "High Score " + PlayerPrefs.GetInt("HighScore").ToString();



	}

	void KeepTime(){
		time = time -1;
		timeText.text = time.ToString();
		if(time == 0){
			StartCoroutine("GameOver","Times Up");
		}
	}

	void OnScore(int points){
		score = score + points;
		scoreText.text = score.ToString();
	}

	void OnScoreTime(){
		time = time + 10;
		if(time > 60){
			time = 60;
		}
		timeText.text = time.ToString();
	}

	void OnScoreBullets (){
		bullets = bullets + 10;
		if(bullets > 100){
			bullets = 100;
		}
		bulletText.text = bullets.ToString();
	}

	void OnShotBullet(){
		if(fxMuted == false){
		fxTrack.PlayOneShot(fxSounds.gunShot);
		}
		bullets = bullets -1;
		bulletText.text = bullets.ToString();
		if(bullets == 0){
			StartCoroutine("GameOver","Out of Ammo");
		}
	}

	void OnBulletHit(string tag){
		if(fxMuted){
			return;
		}
		switch(tag)
		{
		case "metal" : 
			fxTrack.PlayOneShot(fxSounds.metalShot);
			break;
		case "wood" : 
			fxTrack.PlayOneShot(fxSounds.woodShot);
			break;
		case "glass" : 
			fxTrack.PlayOneShot(fxSounds.glassShot);
			break;
		case "can" : 
			fxTrack.PlayOneShot(fxSounds.canShot);
			break;
		}

	}


	void OnCamUp(){
		playPannel.SetActive(true);
		transitionState = TransitionState.up;
		GameOn();
	}

	void OnCamDown(){

	}

	void GameOn(){
		if(fxMuted == false){
		fxTrack.PlayOneShot(fxSounds.gunCock);
		}
		gameState = GameState.playing;
		ResetStats ();
		InvokeRepeating("KeepTime",1f,1f);
		OnStartGame();
	}

	void ResetStats(){
		time = 60;
		timeText.text = time.ToString();
		bullets = 100;
		bulletText.text = bullets.ToString();
		score = 0;
		scoreText.text = score.ToString();

	}

	public void MuteFX(){
		fxMuted = true;
	}


	public void UnMuteFX(){
		fxMuted = false;
	}


	public void MuteMisic(){
		musicMute.TransitionTo(.01f);
	}

	public void UnMuteMusic(){
		musicUnMute.TransitionTo(.01f);
	}




	IEnumerator GameOver(string cause){
		yield return new WaitForEndOfFrame();
		OnGameOver();
		CancelInvoke("KeepTime");
		gameState = GameState.gameOver;
		gameOverPannel.SetActive(true);
		gameOverText.text = cause;
		if(score > PlayerPrefs.GetInt("HighScore")){
			PlayerPrefs.SetInt("HighScore",score);
			newHighScore.text = "New High Score " + score.ToString();
			newHighScore.gameObject.SetActive(true);
		}
	}



	public void PlayAgain(){
		GameOn();
		newHighScore.gameObject.SetActive(false);
	}


}

[System.Serializable]
public class FXSounds{
	public AudioClip metalShot;
	public AudioClip woodShot;
	public AudioClip glassShot;
	public AudioClip gunCock;
	public AudioClip gunShot;
	public AudioClip canShot;
}

[System.Serializable]
public enum TransitionState{
	down,moving,up
}

[System.Serializable]
public enum GameState{
	playing,gameOver
}

