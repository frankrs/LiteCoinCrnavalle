using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Target target;

	public float speed;

	public float life;

	private Animator anim;


	public delegate void Score(int points);
	public static event Score OnScore;

	void Awake () {
		anim = GetComponent<Animator>();
	}

	void Start () {
		Invoke("End",life);
	}

	void End(){
		Destroy(gameObject);
	}

	public void Shot (){
		if(!target.shot){
			anim.SetTrigger("Shot");
			OnScore(target.points);
		}
		target.shot = true;
	}

	//public void Score():


	void Update(){
		transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
	}
}


[System.Serializable]
public class Target{
	public bool shot = false;
	public int points;
}
