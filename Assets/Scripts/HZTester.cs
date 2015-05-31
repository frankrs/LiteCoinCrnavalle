using UnityEngine;
using System.Collections;

public class HZTester : MonoBehaviour {

	public string iD;


	public void Init() {
		HeyzapAds.start(iD, HeyzapAds.FLAG_NO_OPTIONS);
	}

	public void ShowMed(){
		HeyzapAds.showMediationTestSuite();
	}


	void Update(){
		if(Input.GetKeyDown("escape")){
			Application.Quit();
		}
	}

}
