using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public Text scoreText;

	public Text timeText;

	public Text bulletText;

	public static int score;

	public static int time = 60;

	private static int bullets = 100;

	void OnEnable()
	{
		Duck.OnScore += OnScore;
		Controls.OnShotBullet += OnShotBullet;
	}
	
	
	void OnDisable()
	{
		Duck.OnScore -= OnScore;
		Controls.OnShotBullet -= OnShotBullet;
	}

	void Start(){
		InvokeRepeating("KeepTime",1f,1f);
	}

	void KeepTime(){
		time = time -1;
		timeText.text = time.ToString();
	}

	void OnScore(int points){
		score = score + points;
		scoreText.text = score.ToString();
	}

	void OnShotBullet(){
		bullets = bullets -1;
		bulletText.text = bullets.ToString();
	}
}


