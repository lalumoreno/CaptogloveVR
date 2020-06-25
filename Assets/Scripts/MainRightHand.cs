using UnityEngine;

public class MainRightHand : MonoBehaviour
{
	public Transform transRH;

	//Set this by default 
	public Transform transThuR; 
	public Transform transIndR; 
	public Transform transMidR; 
	public Transform transRinR; 
	public Transform transPinR;

	private MyHand 	RightHand;
		
	public Transform myButton;
	public Transform myCapsule;
	public Transform myLever;
	public Transform myLight;
	
	private Transform myCatchObject;
	private Transform myPressedObject;

	private Renderer capsuleRenderer;
	private Renderer buttonRenderer;
	private Renderer leverRenderer;
	private Renderer lightRender;

	private GUIStyle style;
	private bool blights;
	//ASK in SCRIPT IF USE ARM OR NOT 

	// Start is called before the first frame update
	void Start()
    {
		capsuleRenderer = myCapsule.GetComponent<Renderer>();
		buttonRenderer = myButton.GetComponent<Renderer>();
		leverRenderer = myLever.GetComponent<Renderer>();
		lightRender = myLight.GetComponent<Renderer>();
		
		myPressedObject = null;
		myCatchObject = null;

		blights = false; 

		style = new GUIStyle();
		style.normal.textColor = Color.black;
		
		//Do all of this in interface for Unity - values in screen 

		RightHand = new MyHand(2443, true); 
	   //Model must be in flat position 
	   RightHand.SetTransform(transRH, MyModule.ModuleAxis.AXIS_X, MyModule.ModuleAxis.AXIS_Z, MyModule.ModuleAxis.AXIS_Y); //pitch, yaw, roll
	   //RightHand.SetFingerSensors	(1,			2,			3,		4,		5);
	   RightHand.SetFingerModel(transThuR,transIndR,transMidR,transRinR,transPinR, MyModule.ModuleAxis.AXIS_X, 3);//Needs the sensors configuration first
		//Set this values by default 
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
		
		if(RightHand.isCatch() && myCatchObject != null)
		{
			Debug.Log("Right Hand is closed");
			//RightHand.CatchObject(myLever);			
			RightHand.CatchObject(myCatchObject);
		}		/*
		else if (RightHand.isNumber1() && blights) 
		{
			lightRender.material.SetColor("_Color", Color.red);
		}
		else if (RightHand.isNumber2() && blights)
		{
			lightRender.material.SetColor("_Color", Color.yellow);
		}
		else if (RightHand.isNumber3() && blights)
		{
			lightRender.material.SetColor("_Color", Color.green);
		}
		else if (RightHand.isPressed() && myPressedObject != null)
		{
			Debug.Log("Right Button pressed");
			RightHand.PressButton(myPressedObject, new Vector3(-11.88f, 9, 20.06f));
		}
		else if (!RightHand.isPressed())
		{
			RightHand.PressButton(myPressedObject, new Vector3(-11.88f, 9, 19.48f));
		}*/

	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Capsule")
		{
			Debug.Log("OnTriggerEnter!!");
			capsuleRenderer.material.SetColor("_Color", Color.green);
			myCatchObject = myCapsule;
		}
		else if(other.tag == "Button")
		{
			Debug.Log("OnTriggerEnter!!");
			buttonRenderer.material.SetColor("_Color", Color.green);
			myPressedObject = myButton;
		}
		else if (other.tag == "Lever")
		{
			Debug.Log("OnTriggerEnter!!");
			leverRenderer.material.SetColor("_Color", Color.green);
			myCatchObject = myLever;
		}
		else if (other.tag == "Player")
		{
			Debug.Log("OnTriggerEnter!!");
			blights = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Capsule")
		{
			capsuleRenderer.material.SetColor("_Color", Color.red);
			myCatchObject = null;
		}
		else if (other.tag == "Button")
		{
			Debug.Log("OnTriggerEnter!!");
			buttonRenderer.material.SetColor("_Color", Color.red);
			myPressedObject = null;
		}
		else if (other.tag == "Lever")
		{
			Debug.Log("OnTriggerEnter!!");
			leverRenderer.material.SetColor("_Color", Color.red);
			myCatchObject = null;
		}
		else if (other.tag == "Player")
		{
			Debug.Log("OnTriggerEnter!!");
			blights = false; 
		}

	}

	void OnGUI()
	{
		if (RightHand.sensorRead)
		{
			GUI.Label(new Rect(0, 0, 200f, 200f), "Right Hand ready", style);
		}
		if (blights)
		{
			GUI.Label(new Rect(0, 40f , 200f, 200f), "Light box", style);
		}
	}

	private void OnDestroy() {
		RightHand.Stop();
	}
		
}
