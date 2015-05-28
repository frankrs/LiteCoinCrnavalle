using UnityEngine;
using System.Collections;

public class TsetScript : MonoBehaviour {


	void OnMouseDown () {
		gameObject.SendMessage("Shot");
	}
	

}
