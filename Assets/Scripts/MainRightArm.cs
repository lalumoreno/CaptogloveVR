﻿using UnityEngine;

using Arm;

public class MainRightArm : MonoBehaviour
{
	public Transform transRA;
	private MyArm RightArm;

	private GUIStyle style;
	//ASK in SCRIPT IF USE ARM OR NOT 

	// Start is called before the first frame update
	void Start()
	{
		style = new GUIStyle();
		style.normal.textColor = Color.black;

		//Do all of this in interface for Unity - values in screen 
		RightArm = new MyArm(2469, true);
		RightArm.SetTransform(transRA, MyModule.ModuleAxis.AXIS_Y, MyModule.ModuleAxis.AXIS_Z, MyModule.ModuleAxis.AXIS_X);
		RightArm.Pitch(-90, 90);
		RightArm.Yaw(0, -180);

		RightArm.Start();
	}

	// Update is called once per frame
	void Update()
	{
		RightArm.Wirst();
	}

	void OnGUI()
	{
		if (RightArm.sensorRead)
		{
			GUI.Label(new Rect(0,0+20f, 200f, 200f), "Right Arm ready", style);
		}
	}

	private void OnDestroy()
	{
		RightArm.Stop();
	}

}