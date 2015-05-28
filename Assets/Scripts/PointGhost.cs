using UnityEngine;
using System.Collections;

public class PointGhost : MonoBehaviour {

	public float life = 1f;

	public float floatSpeed;

	public TextMesh rend;

	IEnumerator Start () {
		while (life > 0f){
			//rend.color = new Vector4(rend.color.r,rend.color.g,rend.color.b,life);
			life = life - Time.deltaTime;
			transform.Translate(transform.up * floatSpeed,Space.World);
			yield return new WaitForEndOfFrame();
		}
		Destroy (gameObject);
	}
	

}
