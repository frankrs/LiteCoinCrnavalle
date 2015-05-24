using UnityEngine;
using System.Collections;

public class DuckFactory : MonoBehaviour {

	public GameObject[] targets;

	public float genTime;

	// Use this for initialization
	void Start () {
		InvokeRepeating("GenerateTarget",genTime,genTime);
	}
	
	void GenerateTarget(){
		GameObject t;
		t = GameObject.Instantiate(targets[Random.Range(0,targets.Length)],transform.position,transform.rotation) as GameObject;
	}
}
