using UnityEngine;

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

		LeftHand = new  MyHand(2492, MyHand.eHandType.TYPE_RIGHT_HAND);
		LeftHand.SetHandTransform(transLH, Module.eModuleAxis.AXIS_X, Module.eModuleAxis.AXIS_Z, Module.eModuleAxis.AXIS_Y); //pitch, yaw, roll
		LeftHand.SetFingerTransform(transThuL,transIndL,transMidL,transRinL, transPinL, Module.eModuleAxis.AXIS_X, 3);
	
/*
		 LeftArm  = new MyArm(26, false); 
		 LeftArm.SetHandTransform(transLA, SetModule.eModuleAxis.AXIS_Z, SetModule.eModuleAxis.AXIS_X, SetModule.eModuleAxis.AXIS_Y); 
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
