using UnityEngine;
using GITEICaptoglove;

public class MainLeftHand : MonoBehaviour
{
	//Assign transforms in Unity editor
	public Transform tThumb;
	public Transform tIndex;
	public Transform tMiddle;
	public Transform tRing;
	public Transform tPinky;
	public GameObject ArmObject;

	public myButton button;
	public myCapsule capsule;
	
	private MyHand LeftHand = null;
	private GUIStyle style;

	//Enables-Disables arm in Unity Editor
	public bool UseArm = false;

	// Start is called before the first frame update
	void Start()
	{
		//Configuration for Captoglove sensor as Left Arm
		LeftHand = new  MyHand(2492, MyHand.HandType.TYPE_LEFT_HAND);
		LeftHand.SetLogEnabled(true);
		LeftHand.SetHandObject(transform);
		LeftHand.SetFingerObject(tThumb,tIndex,tMiddle,tRing, tPinky);

		//Needed to enable Captoglove sensor as Left Arm
		if (UseArm)
			ArmObject.GetComponent<MainLeftArm>().enabled = true;
		else
			ArmObject.GetComponent<MainLeftArm>().enabled = false;

		//Messages in display
		style = new GUIStyle();
		style.normal.textColor = Color.black;

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

		//Interaction with capsule 
		if (LeftHand.HandClosed() && capsule.bCollided)
		{
			CatchCapsule();
		}
		//Interaction with button
		/*
		else if (LeftHand.SensorPressed() && button.bCollided)
		{
			button.PressButton();
		}
		else if (!LeftHand.SensorPressed())
		{
			button.ReleaseButton();
		}*/

	}
	private void CatchCapsule()
	{
		Vector3 vMiddlePos = LeftHand.GetMiddlePosition();
		capsule.ChangePosition(new Vector3(vMiddlePos.x, vMiddlePos.y, vMiddlePos.z + 1));
		capsule.ChangeRotation(LeftHand.GetHandRotation());
	}

    void OnGUI()
	{
		if (LeftHand.GetPropertiesRead())
		{
			GUI.Label(new Rect(0, 0, 200f, 200f), "Left Hand ready", style);
		}
	}

	private void OnDestroy()
	{
		if(LeftHand!=null)
			LeftHand.Stop();		
	}

}
