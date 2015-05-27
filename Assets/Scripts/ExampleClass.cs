using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	public Vector2 scrolling;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void OnGUI() {
		if (rend.sharedMaterial as ProceduralMaterial) {
			Rect windowRect = new Rect(Screen.width - 250, 30, 220, Screen.height - 60);
			GUI.Window(0, windowRect, ProceduralPropertiesGUI, "Procedural Properties");
		}
	}
	void ProceduralPropertiesGUI(int windowId) {
		scrolling = GUILayout.BeginScrollView(scrolling);
		ProceduralMaterial substance = rend.sharedMaterial as ProceduralMaterial;
		ProceduralPropertyDescription[] inputs = substance.GetProceduralPropertyDescriptions();
		int i = 0;
		while (i < inputs.Length) {
			ProceduralPropertyDescription input = inputs[i];
			ProceduralPropertyType type = input.type;
			if (type == ProceduralPropertyType.Boolean) {
				bool inputBool = substance.GetProceduralBoolean(input.name);
				bool oldInputBool = inputBool;
				inputBool = GUILayout.Toggle(inputBool, input.name);
				if (inputBool != oldInputBool)
					substance.SetProceduralBoolean(input.name, inputBool);
				
			} else
				if (type == ProceduralPropertyType.Float)
				if (input.hasRange) {
					GUILayout.Label(input.name);
					float inputFloat = substance.GetProceduralFloat(input.name);
					float oldInputFloat = inputFloat;
					inputFloat = GUILayout.HorizontalSlider(inputFloat, input.minimum, input.maximum);
					if (inputFloat != oldInputFloat)
						substance.SetProceduralFloat(input.name, inputFloat);
					
				}
			else
				if (type == ProceduralPropertyType.Vector2 || type == ProceduralPropertyType.Vector3 || type == ProceduralPropertyType.Vector4)
					Debug.Log(input.name);
				if (input.hasRange) {
					GUILayout.Label(input.name);
					int vectorComponentAmount = 4;
					if (type == ProceduralPropertyType.Vector2)
						vectorComponentAmount = 2;
					
					if (type == ProceduralPropertyType.Vector3)
						vectorComponentAmount = 3;
					
					Vector4 inputVector = substance.GetProceduralVector(input.name);
					Vector4 oldInputVector = inputVector;
					int c = 0;
					while (c < vectorComponentAmount) {
						inputVector[c] = GUILayout.HorizontalSlider(inputVector[c], input.minimum, input.maximum);
						c++;
					}
					if (inputVector != oldInputVector)
						substance.SetProceduralVector(input.name, inputVector);
					
				}


//			else
//			if (type == ProceduralPropertyType.Color3 || type == ProceduralPropertyType.Color4) {
//				GUILayout.Label(input.name);
//				int colorComponentAmount = ((type == ProceduralPropertyType.Color3) ? 3 : 4);
//				Color colorInput = substance.GetProceduralColor(input.name);
//				Color oldColorInput = colorInput;
//				int d = 0;
//				while (d < colorComponentAmount) {
//					colorInput[d] = GUILayout.HorizontalSlider(colorInput[d], 0, 1);
//					d++;
//				}
//				if (colorInput != oldColorInput)
//					substance.SetProceduralColor(input.name, colorInput);
//				} 




			else
			if (type == ProceduralPropertyType.Enum) {
				GUILayout.Label(input.name);
				int enumInput = substance.GetProceduralEnum(input.name);
				int oldEnumInput = enumInput;
				string[] enumOptions = input.enumOptions;
				enumInput = GUILayout.SelectionGrid(enumInput, enumOptions, 1);
				if (enumInput != oldEnumInput)
					substance.SetProceduralEnum(input.name, enumInput);
				
			}
			i++;
		}
		substance.RebuildTextures();
		GUILayout.EndScrollView();
	}
}
