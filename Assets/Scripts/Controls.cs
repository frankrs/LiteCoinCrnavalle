using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

	public Joystick joyStick;

	public Vector2 jsPos;

	public RectTransform sight;

	public Vector2 screenSizeHalf;

	public float sightSmooth;

	private Vector2 realScreenPos;

	private Vector2 sightVel;


	public delegate void ShotBullet();
	public static event ShotBullet OnShotBullet;

	public delegate void BulletHit(string tag);
	public static event BulletHit OnBulletHit;


	void Awake(){
		screenSizeHalf = new Vector2 (Screen.width,Screen.height);
	}

	void LateUpdate (){


		if(joyStick.isActiveAndEnabled){
		jsPos = joyStick.JoystickPosition;
		//jsPos = Input.acceleration;
		realScreenPos = new Vector2 (jsPos.x * screenSizeHalf.x, jsPos.y * screenSizeHalf.y);
		sight.anchoredPosition = Vector2.SmoothDamp(sight.anchoredPosition,realScreenPos/2,ref sightVel,sightSmooth);
		}



		if(Input.GetKeyDown("escape")){
			ExitGame();
		}
	}

	public void Shoot (){
		if(ScoreKeeper.bullets < 1 || ScoreKeeper.gameState == GameState.gameOver){
			return;
		}
		OnShotBullet();
		Ray ray = Camera.main.ScreenPointToRay(sight.position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			hit.transform.SendMessage("Shot",SendMessageOptions.DontRequireReceiver);
			OnBulletHit(hit.transform.tag);
		}
		else
			print("I'm looking at nothing!");
		
	}
	
	
	public void ExitGame(){
		Application.Quit();
	}



	public void PlayButton(){
		ScoreKeeper.transitionState = TransitionState.moving;
		Camera.main.gameObject.SendMessage("MoveUp");
	}



}
