using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Target target;

	public float speed;

	public float life;

	public Gradient color;

	public float colorChangeSpeed;

	public Renderer duckRend;

	private Animator anim;

	public float cycleTime = 10;

	private float dialoffset;

	public float dial;

	public GameObject pointGhost;

	public delegate void Score(int points);
	public static event Score OnScore;



	

	void Awake () {
		anim = GetComponent<Animator>();
		duckRend.material.SetTextureOffset("_MetalSpots", new Vector2(Random.Range(0f,1f),Random.Range(0f,1f)));
	}

	void Start () {
		Invoke("End",life);
		dialoffset = Random.Range(0f,2f);
	}

	void End(){
		Destroy(gameObject);
	}

	public void Shot (){
		if(!target.shot){
			anim.SetTrigger("Shot");

			int points = 1;

			if(dial < .15f){
				points = 3;
			}
			else if(dial > .85){
				points = 5;
			}


			OnScore(points);

			// instantiate sighn
			GameObject ghost = GameObject.Instantiate(pointGhost,transform.position + new Vector3 (0f,.1f,.1f),Quaternion.identity) as GameObject;
			ghost.GetComponent<TextMesh>().text = "+" + points.ToString();

		}
		target.shot = true;
	}

	


	void Update(){

		// move the duck forward
		transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);

		// ocsillate color value
		dial = Mathf.PingPong((Time.time + dialoffset) * colorChangeSpeed, 1f);

		// set ducks color
		duckRend.material.SetColor("_Paint",color.Evaluate(dial));
	}

	
}


[System.Serializable]
public class Target{
	public bool shot = false;
	public int points;
}
