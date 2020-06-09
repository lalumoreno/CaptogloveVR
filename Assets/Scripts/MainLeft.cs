using UnityEngine;

using Arm;

public class MainLeft : MonoBehaviour
{
	public Transform transLH;

	public Transform transThuL;
	public Transform transIndL;
	public Transform transMidL;
	public Transform transRinL;
	public Transform transPinL;

	//public Transform transLA;
	private MyHand LeftHand;

	// Start is called before the first frame update
	void Start()
	{
		//Do all of this in interface for Unity - values in screen 

		LeftHand = new  MyHand(2492, false);
		LeftHand.SetTransform(transLH, MyModule.ModuleAxis.AXIS_X, MyModule.ModuleAxis.AXIS_Z, MyModule.ModuleAxis.AXIS_Y); //pitch, yaw, roll
		LeftHand.SetFingerModel(transThuL,transIndL,transMidL,transRinL, transPinL, MyModule.ModuleAxis.AXIS_X, 3);
		LeftHand.Pitch	(-90,90); 
		LeftHand.Yaw	(-90,90); 
		LeftHand.Roll	(-180,180);
		LeftHand.Thumb	(12.681f, 60, -0.992f, 50, 6.269001f, 50);
		LeftHand.Index	(21.155f, 80,  -5.408f, 75, -2.203f, 75);	   
		LeftHand.Middle	(24.201f, 80, -10.915f, 75,  3.174f, 75);
		LeftHand.Ring	(24.854f,  80, -10.759f, 75,  0.541f, 75);
		LeftHand.Pinky	(22.229f, 80,  -5.971f, 75, -5.211f, 75);
		LeftHand.setShakeDegree(2);
/*
		 LeftArm  = new MyArm(26, false); 
		 LeftArm.SetTransform(transLA, Module.ModuleAxis.AXIS_Z, Module.ModuleAxis.AXIS_X, Module.ModuleAxis.AXIS_Y); 
		 */
		LeftHand.Start();
		//RightArm.Start();
		//LeftArm.Start();

	}

	// Update is called once per frame
	void Update()
	{
		LeftHand.Wirst();
		LeftHand.Fingers();

		if (LeftHand.isPressed())
		{
			Debug.Log("Left Button pressed");
		}

		if (LeftHand.isCatch())
		{
			Debug.Log("Left Hand is closed");
		}

		if (LeftHand.isNumber1())
		{
			Debug.Log("Left Hand is number 1");
		}

	}

	private void OnDestroy()
	{
		LeftHand.Stop();
		//LeftArm.Stop();
	}

}
