using UnityEngine;
using GITEICaptoglove;

public class MainRightHand : MonoBehaviour
{
    //Assign transforms in Unity editor      
    public Transform tThumb;
    public Transform tIndex;
    public Transform tMiddle;
    public Transform tRing;    
    public Transform tPinky;
    public GameObject ArmObject; 

    public myButton button;    
    public myLever lever;
    public myLight mylight;
    public myCapsule capsule;

    private MyHand RightHand = null;     
    private GUIStyle style;      

    //Enables-Disables arm in Unity Editor
    public bool UseArm = false;

    // Start is called before the first frame update
    void Start()
    {
        //Add CaptoGlove module as right hand sensor
        RightHand = new MyHand(2443, MyHand.HandType.TYPE_RIGHT_HAND);
        RightHand.SetLogEnabled(true);
        RightHand.SetHandObject(transform);
        RightHand.SetFingerObject(tThumb, tIndex, tMiddle, tRing, tPinky);

        //Needed to enable Captoglove sensor as Right Arm
        if (UseArm)
            ArmObject.GetComponent<MainRightArm>().enabled = true; 
        else
            ArmObject.GetComponent<MainRightArm>().enabled = false;

        //Messages in display
        style = new GUIStyle();
        style.normal.textColor = Color.black;        
        
        //Starts Captoglove sensor 
        RightHand.Start();        
    }

    // Update is called once per frame
    void Update()
    {   
        //Enables hand movement 
        if (UseArm)
            RightHand.MoveHandNoYaw();
        else
            RightHand.MoveHand();

        //Enables finger movement
        RightHand.MoveFingers();        

        //Interaction with capsule 
        if (RightHand.HandClosed() && capsule.bCollided)
        {                      
            CatchCapsule();
        }
        //Interaction with myLever
        else if (RightHand.HandClosed() && lever.bCollided)
        {
            CatchLever();
        }
        //Ineraction with mylight
        else if (RightHand.FingerGesture1() && mylight.bCollided) 
		{
            mylight.cColor = Color.blue;            
		}        
        else if (RightHand.FingerGesture2() && mylight.bCollided)
		{
            mylight.cColor = Color.yellow;            
        }        
        else if (RightHand.FingerGesture3() && mylight.bCollided)
		{
            mylight.cColor = Color.green;            
        }
        //Ineraction with button
        else if (RightHand.SensorPressed() && button.bCollided)                
        {
            button.PressButton();
        }
		else if (!RightHand.SensorPressed())
		{
            button.ReleaseButton();
        }

    }

    private void CatchCapsule()
    {
        Vector3 vMiddlePos = RightHand.GetMiddlePosition();
        capsule.ChangePosition(new Vector3(vMiddlePos.x, vMiddlePos.y, vMiddlePos.z + 1));
        capsule.ChangeRotation(RightHand.GetHandRotation());        
    }

    private void CatchLever()
    {
        Vector3 vHandPos = RightHand.GetHandPosition();

        if ((vHandPos.y - 1) > 7.5f &&
            (vHandPos.y - 1) < 12f)
        {
            lever.ChangePosition( vHandPos.y - 1);
        }
    }

    void OnGUI()
    {        
        if (RightHand.GetPropertiesRead())
        {
            GUI.Label(new Rect(Screen.width-100, 0, 200f, 200f), "Right Hand ready", style);
        }
    }

    private void OnDestroy()
    {
        if(RightHand!= null)
            RightHand.Stop();
    }

}
