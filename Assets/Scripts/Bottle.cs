using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

	public float speed = 7.5f;

	public float life;

	public GameObject ghostGO;


	public delegate void ScoreTime();
	public static event ScoreTime OnScoreTime;
	
	
	void Start () {
		Invoke("End",life);
	}

	void End(){
		Destroy(gameObject);
	}
	
	void Update () {
		// move the duck forward
		transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
//		if(Input.GetKeyDown("space")){
//			Shot ();
//		}
	}


	public void Shot (){
		GetComponent<ParticleSystem>().Emit(100);
		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;

		GameObject ghost = GameObject.Instantiate(ghostGO,transform.position + new Vector3 (0f,.1f,.1f),Quaternion.identity) as GameObject;
		ghost.GetComponent<TextMesh>().text = "+ Time";
		OnScoreTime();
	}
	

	void OnMouseDown (){
		Shot ();
	}

}
