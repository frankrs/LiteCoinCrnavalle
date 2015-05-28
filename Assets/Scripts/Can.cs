using UnityEngine;
using System.Collections;

public class Can : MonoBehaviour {

	public float speed = .75f;
	
	public float life = 9f;
	
	public GameObject ghostGO;

	public bool targetShot = false;

	public float shotForce = 10f;
	
	public delegate void ScoreBullets();
	public static event ScoreBullets OnScoreBullets;
	
	
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

		if(targetShot){
			return;
		}
		targetShot = true;
		
		GameObject ghost = GameObject.Instantiate(ghostGO,transform.position + new Vector3 (0f,.1f,.1f),Quaternion.identity) as GameObject;
		ghost.GetComponent<TextMesh>().text = "+ Ammo";
		Rigidbody rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
		rb.AddForce(new Vector3(Random.Range(-1f,1f),Random.Range(1f,4f),Random.Range(-1f,1f)) * shotForce, ForceMode.Impulse);
		OnScoreBullets();
	}
	
	
//		void OnMouseDown (){
//			Shot ();
//		}
}
