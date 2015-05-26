using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour {

	public delegate void CamUp();
	public static event CamUp OnCamUp;

	public delegate void CamDown();
	public static event CamDown OnCamDown;

	public float camUpy;

	public float camDowny;

	public AnimationCurve moveCurve;

	//public float

	public void MoveUp (){
		StartCoroutine("MovingUp");
	}

	IEnumerator MovingUp (){
		float time = 0f;
		while(time < 1f){
			time = time + Time.deltaTime;
			transform.position = new Vector3 (transform.position.x,moveCurve.Evaluate(time),transform.position.z);
			yield return new WaitForEndOfFrame();
		}
		OnCamUp();
	}


}
