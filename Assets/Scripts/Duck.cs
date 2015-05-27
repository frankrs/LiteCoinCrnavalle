using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Target target;

	public float speed;

	public float life;

	public Gradient color;

	public float colorChangeSpeed;

	public Renderer duckRend;

	private ProceduralMaterial sub;

	private Animator anim;

	public float cycleTime = 10;

	private float dialoffset;

	public float dial;

	public delegate void Score(int points);
	public static event Score OnScore;

	

	void Awake () {
		anim = GetComponent<Animator>();
		sub = duckRend.material as ProceduralMaterial;
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
			OnScore(target.points);
		}
		target.shot = true;
	}


	void Update(){
		// move the duck forward
		transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
	}



	void LateUpdate(){

		// ocsillate color value
		dial = Mathf.PingPong((Time.time + dialoffset) * colorChangeSpeed, 1f);

			if(sub){
	
				Color32 col;
	
				col = color.Evaluate(dial);;
	
				sub.SetProceduralColor("Paint",col);
				sub.RebuildTextures();
			}

	}
}


[System.Serializable]
public class Target{
	public bool shot = false;
	public int points;
}
