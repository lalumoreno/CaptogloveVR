using UnityEngine;

public class MainLeftHand : MonoBehaviour
{
	//Assign transforms in Unity editor
	public Transform tHand;
	public Transform tThumb;
	public Transform tIndex;
	public Transform tMiddle;
	public Transform tRing;
	public Transform tPinky;
	public GameObject ArmObject;

	private MyHand LeftHand = null;

	//Enables-Disables arm in Unity Editor
	public bool UseArm = false;

	// Start is called before the first frame update
	void Start()
	{
		//Configuration for Captoglove sensor as Left Arm
		LeftHand = new  MyHand(2492, MyHand.eHandType.TYPE_LEFT_HAND);
		LeftHand.EnableLog();
		LeftHand.SetHandTransform(tHand, Module.eModuleAxis.AXIS_X, Module.eModuleAxis.AXIS_Z, Module.eModuleAxis.AXIS_Y); //pitch, yaw, roll
		LeftHand.SetFingerTransform(tThumb,tIndex,tMiddle,tRing, tPinky);

		//Needed to enable Captoglove sensor as Left Arm
		if (UseArm)
			ArmObject.GetComponent<MainLeftArm>().enabled = true;
		else
			ArmObject.GetComponent<MainLeftArm>().enabled = false;

		//Starts Captoglove sensor 
		LeftHand.Start();
	}

	// Update is called once per frame
	void Update()
	{
		//Enables hand movement 
		if (UseArm)
			LeftHand.MoveHandNoYaw();
		else
			LeftHand.MoveHand();

		//Enables finger movement
		LeftHand.MoveFingers();

	}

	private void OnDestroy()
	{
		if(LeftHand!=null)
			LeftHand.Stop();		
	}

}
