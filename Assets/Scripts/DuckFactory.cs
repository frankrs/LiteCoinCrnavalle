using UnityEngine;
using System.Collections;

public class DuckFactory : MonoBehaviour {

	public GameObject[] targets;

	public float genTime;


	void OnEnable(){
		ScoreKeeper.OnStartGame += OnStartGame;
		ScoreKeeper.OnGameOver += OnGameOver;
	}

	void OnDisable(){
		ScoreKeeper.OnStartGame -= OnStartGame;
	}

	void OnStartGame () {
		InvokeRepeating("GenerateTarget",genTime,genTime);
	}

	void OnGameOver(){
		CancelInvoke("GenerateTarget");
	}

	void GenerateTarget(){
		GameObject t;
		t = GameObject.Instantiate(targets[Random.Range(0,targets.Length)],transform.position,transform.rotation) as GameObject;
	}
}
