using UnityEngine;

using Arm; 

public class MainRightHand : MonoBehaviour
{
	public Transform transRH;

	public Transform transThuR; 
	public Transform transIndR; 
	public Transform transMidR; 
	public Transform transRinR; 
	public Transform transPinR;
	
	private MyHand 	RightHand;
		
	public Transform[] myObjects;
	public Transform myButton;

	private int i; 
	private GUIStyle style;

	private float nextActionTime = 0.0f;
	public float period = 0.1f;
	//ASK in SCRIPT IF USE ARM OR NOT 

	// Start is called before the first frame update
	void Start()
    {
		style = new GUIStyle();
		style.normal.textColor = Color.black;
		i = 0;
		//Do all of this in interface for Unity - values in screen 

		RightHand = new MyHand(2443, true); 
	   //Model must be in flat position 
	   RightHand.SetTransform(transRH, MyModule.ModuleAxis.AXIS_X, MyModule.ModuleAxis.AXIS_Z, MyModule.ModuleAxis.AXIS_Y); //pitch, yaw, roll
	   //RightHand.SetFingerSensors	(1,			2,			3,		4,		5);
	   RightHand.SetFingerModel(transThuR,transIndR,transMidR,transRinR,transPinR, MyModule.ModuleAxis.AXIS_X, 3);//Needs the sensors configuration first
	   RightHand.Pitch	( 90,-90); //right left
	   RightHand.Yaw	( 90,-90); //right left 
	   RightHand.Roll	(-180,180); 
	   RightHand.Thumb	(-9.475f,  -60, -6.888f,  -50, -6.334f,  -50); 
	   RightHand.Index	(-23.606f, -80, 5.069f, -75,  2.359f, -75 ); 	   
	   RightHand.Middle	(-26.575f, -80, 10.864f,-75, -3.127f, -75); 
	   RightHand.Ring	(-27.302f, -80, 11.405f,-75, -1.038f, -75); 
	   RightHand.Pinky	(-24.763f, -80, 6.326f, -75,  5.373f, -75 ); 
	   RightHand.setShakeDegree(2);	   
		
       RightHand.Start(); //To start , the model is not needed 		
    }

    // Update is called once per frame
    void Update()
    {		
		RightHand.Wirst();			
		RightHand.Fingers();

		if (RightHand.isPressed())
		{
			Debug.Log("Right Button pressed");
			RightHand.PressButton(myButton, new Vector3(11.11f, 9, 20.22f));
		}
		else {
			RightHand.PressButton(myButton, new Vector3(11.11f, 9, 19.48f));
		}
		

		if(RightHand.isCatch()){
			Debug.Log("Right Hand is closed");
			//RightHand.CatchObject(myLever);			
			RightHand.CatchObject(myObjects[i]);
		}
		
		if(RightHand.isNumber2()){

			if (Time.time > nextActionTime)
			{
				Debug.Log("Right Hand is number 2");
				nextActionTime = Time.time + period;
				// execute block of code here				
				if (i < myObjects.Length)
					i++;
				else
					i = 0;
			}
		}

    }

	void OnGUI()
	{
		if (RightHand.sensorRead)
		{
			GUI.Label(new Rect(0, 0/ 2, 200f, 200f), "Right Hand ready", style);
		}
		if (RightHand.isCatch())
		{
			GUI.Label(new Rect(Screen.width / 3, Screen.height / 3, 200f, 200f), "Object is catch", style);
		}
		if (RightHand.isNumber2())
		{
			GUI.Label(new Rect(Screen.width / 3, Screen.height / 3, 200f, 200f), "Object changed", style);
		}
	}

	private void OnDestroy() {
		RightHand.Stop();
	}
		
}
