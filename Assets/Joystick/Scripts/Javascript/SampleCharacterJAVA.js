/* Written by Kaz Crowe */
#pragma strict

/* Transform's and Movement Variables */
private var myTransform : Transform;
var followJoystick : Transform;
var playerCamera : Transform;
var speed : float = 0.1f;
private var followDistance : Vector3 = new Vector3( 0, 6, -7.5f );
private var rotationSpeed : int = 10;
	/* Joystick's */
var joystickLeft : JoystickJAVA;
var joystickRight : JoystickJAVA;


function Start () 
{
	// We will store our transform for easier reference
	myTransform = this.transform;
}

function Update ()
{
	// In order to use our joystick, we will call our 
	// JoystickPosition which will return our Joystick's
	// position as a Vector2.
	var joystickLeftPos : Vector2 = joystickLeft.JoystickPosition; 
	var joystickRightPos : Vector2 = joystickRight.JoystickPosition;
	
	// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
	if( joystickLeftPos != Vector2.zero )
	{
		// This moves our character
		var movement : Vector3 = new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y );
		myTransform.Translate( movement * speed );
	}
	
	// This will follow our player's position
	followJoystick.position = myTransform.position;
	
	// If we are touching our right joystick, then we want to follow it
	if( joystickRightPos != Vector2.zero )
	{
		// Store our joystick positions into a temporary Vector3
		var facingDirection : Vector3 = new Vector3( joystickRightPos.x, 0, joystickRightPos.y );
		
		// Store that Vector3 we just made into a Quaternion by using LookRotation
		var targetRotation : Quaternion = Quaternion.LookRotation( facingDirection );

		// Now we will use Quaternion.Slerp to get a more smooth transition
		followJoystick.rotation = Quaternion.Slerp( followJoystick.rotation, targetRotation, Time.deltaTime * rotationSpeed );
	}
	
	// If our right joystick is not being touched, then we want to follow our left joystick
	else if( joystickLeftPos != Vector2.zero )
	{
		// Now we repeat the steps above
		var facingDirectionLft : Vector3 = new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y );
		var targetRotationLft : Quaternion = Quaternion.LookRotation( facingDirectionLft );
		followJoystick.rotation = Quaternion.Slerp( followJoystick.rotation, targetRotationLft, Time.deltaTime * rotationSpeed );
	}
	
	// This will make the camera follow our player
	CameraFollow();
}

function CameraFollow ()
{
	playerCamera.position = myTransform.position + followDistance;
	playerCamera.LookAt( myTransform );
}