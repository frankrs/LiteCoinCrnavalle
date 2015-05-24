/* Written by Kaz Crowe */
#pragma strict

public class JoystickJAVA extends MonoBehaviour implements UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IDragHandler, UnityEngine.EventSystems.IPointerUpHandler
{
	/* Positioning */
	enum Position
	{
		Left,
		Right
	}
	var position : Position;
	var joystick : Transform;
	private var joystickCenter : Vector3;
	private var radius : float;
	/* Error Checking */
	private var displayTension : boolean = false;
	private var displayHL : boolean = false;
	/* Coloring */
	var highlightColor : Color;
	var highlightImage : UnityEngine.UI.Image[];
	var tensionAccentColor : Color;
	var tensionAccentUp : UnityEngine.UI.Image;
	var tensionAccentDown : UnityEngine.UI.Image;
	var tensionAccentLeft : UnityEngine.UI.Image;
	var tensionAccentRight : UnityEngine.UI.Image;
	private var defaultTensionAccentColor : Color;
	private var alphaModifier : float;


	function Start ()
	{
		// If you are using a Joystick texture that does not support TensionAccent, Highlight, or JoystickHL,
		// Then you can leave those variables unassigned, and just comment out the ErrorCheck();
		ErrorCheck();
		// This configures our texture size and position so that it looks correct for any screen size
		var textureSize : float = Screen.height / 3;
		var texturePosition : Vector2 = ConfigurePosition( textureSize );
		radius = textureSize / 2.5f;

		// Now let's change our transform so that all of our components will be the correct size.
		// NOTE: In order for this to work, you will need to make all children of this object
		// fill the screen for their RectTransform.
		var baseTrans : RectTransform = GetComponent( RectTransform );
		baseTrans.sizeDelta = new Vector2( textureSize, textureSize );
		baseTrans.position = texturePosition;

		// Store the joystick center so we can return to it
		joystickCenter = joystick.position;

		// Set up our colors and then reset them to default
		alphaModifier = tensionAccentColor.a;
		defaultTensionAccentColor = tensionAccentColor;
		defaultTensionAccentColor.a = 0.0f;
		if( displayTension == true )
			TensionAccentReset();
		if( displayHL == true )
		{
			for( var i : int = 0; i < highlightImage.Length; i++ )
			{
				highlightImage[ i ].color = highlightColor;
			}
		}
	}
	
	function get JoystickPosition () : Vector2
	{
		var tempVec : Vector2 = joystick.position - joystickCenter;
		return tempVec / radius;
	}
	
	function OnPointerDown ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// This means we have touched, so process where we have touched
		UpdateJoystick( touchInfo );
	}
	
	function OnDrag ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// This means we are moving, so process where we are touching
		UpdateJoystick( touchInfo );
	}
	
	function OnPointerUp ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// This means we have let go, so reset to our joystick center
		joystick.position = joystickCenter;
		if( displayTension == true )
			TensionAccentReset();
	}
	
	function UpdateJoystick ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// Create a new Vector2 to equal the vector from our curret touch to the center of joystick
		var tempVector : Vector2 = touchInfo.position - joystickCenter;

		// Clamp the vector to our selected radius 
		tempVector = Vector2.ClampMagnitude( tempVector, radius );

		// Apply the position to our joystick
		joystick.transform.position = joystickCenter + tempVector;

		// This will display our joystick tension if we have all of them
		if( displayTension == true )
			TensionAccentDisplay();
	}
	
	function TensionAccentDisplay ()
	{
		// We need a Vector2 to store our joystickPosition
		var tension : Vector2 = JoystickPosition;

		// Now we will use 2 floats to control amount of tension projected
		var tensionX : float = tension.x * alphaModifier;
		var tensionY : float = tension.y * alphaModifier;

		// And we need a new color to work with
		var tensionColor : Color = defaultTensionAccentColor;

		// If our joystick is to the right
		if( tensionX > 0 )
		{
			// Then we set our color.a according to our X position
			tensionColor.a = tensionX;
			tensionAccentRight.color = tensionColor;

			// If our tensionAccentLeft is not defaultTensionAccentColor, we want to make it defaultTensionAccentColor
			if( tensionAccentLeft.color != defaultTensionAccentColor )
				tensionAccentLeft.color = defaultTensionAccentColor;
		}
		// else our joystick is to the left
		else
		{
			// Then we set our color.a according to our X position
			// We multiply by -1 because our X is currently negative, and we need a positive number to work with
			tensionColor.a = tensionX * -1;
			tensionAccentLeft.color = tensionColor;

			// If our tensionAccentRight is not defaultTensionAccentColor, we want to make it defaultTensionAccentColor
			if( tensionAccentRight.color != defaultTensionAccentColor )
				tensionAccentRight.color = defaultTensionAccentColor;
		}
		if( tensionY > 0 )
		{
			// Then we set our color.a according to our Y position
			tensionColor.a = tensionY;
			tensionAccentUp.color = tensionColor;

			// If our tensionAccentDown is not defaultTensionAccentColor, we want to make it defaultTensionAccentColor
			if( tensionAccentDown.color != defaultTensionAccentColor )
				tensionAccentDown.color = defaultTensionAccentColor;
		}
		else
		{
			// Then we set our color.a according to our Y position
			// We multiply by -1 because our Y is currently negative, and we need a positive number to work with
			tensionColor.a = tensionY * -1;
			tensionAccentDown.color = tensionColor;

			// If our tensionAccentUp is not defaultTensionAccentColor, we want to make it defaultTensionAccentColor
			if( tensionAccentUp.color != defaultTensionAccentColor )
				tensionAccentUp.color = defaultTensionAccentColor;
		}
	}
	
	function TensionAccentReset ()
	{
		// This resets our tension colors back to default
		tensionAccentUp.color = defaultTensionAccentColor;
		tensionAccentDown.color = defaultTensionAccentColor;
		tensionAccentLeft.color = defaultTensionAccentColor;
		tensionAccentRight.color = defaultTensionAccentColor;
	}
	
	function ConfigurePosition ( textureSize : float ) : Vector2
	{
		// This configures our UI positions according to what our position variable is set as
		var positionX : float;
		var positionY : float;
		var positionSpacer : float = textureSize / 4;
		if( position == Position.Left )
		{
			positionX = ( Screen.width / 2 ) / Screen.width + positionSpacer;
		}
		else
		{
			positionX = ( Screen.width - textureSize ) - positionSpacer;
		}
		positionY = ( Screen.height / 2 ) / Screen.height + positionSpacer;
		return new Vector2( positionX, positionY );
	}
	
	function ErrorCheck ()
	{
		// Here we are checking if any of our tensionAccent's are unassigned
		// And if they are, we will turn them off and not display tension
		if( tensionAccentUp == null || tensionAccentDown == null || tensionAccentLeft == null || tensionAccentRight == null )
		{
			// We want to send a Warning, so that we know why we aren't having tension displayed
			Debug.LogWarning( "WARNING: One or more of the Tension Accent Image's are unassigned. Tension Accents will not be displayed to avoid errors." );
			displayTension = false;
		}
		else
			displayTension = true;

		// Check for any highlight Image's
		if( highlightImage.Length == 0 )
		{
			Debug.LogWarning( "WARNING: No Highlight Image(s) are assigned! No highlights will be displayed." );
			displayHL = false;
		}
		else
			displayHL = true;
	}
}