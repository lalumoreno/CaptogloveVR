using UnityEngine;

public class MainRightHand : MonoBehaviour
{
    //Assign transforms in Unity editor
    public Transform tHand;    
    public Transform tThumb;
    public Transform tIndex;
    public Transform tMiddle;
    public Transform tRing;    
    public Transform tPinky;
    public GameObject ArmObject; 

    public Transform tButton;
    public Transform tCapsule;
    public Transform tLever;
    public Transform tLigth;

    private MyHand RightHand = null;

    private Renderer rCapsule;
    private Renderer rButton;
    private Renderer rLever;
    private Renderer rLigth;

    private GUIStyle style;    

    private bool bCapsule = false;
    private bool bLight = false;
    private bool bLever = false;
    private bool bButton = false;

    //Enables-Disables arm in Unity Editor
    public bool UseArm = false;  

    // Start is called before the first frame update
    void Start()
    {
        //Configuration for Captoglove sensor as Right Hand 
        RightHand = new MyHand(2443, MyHand.eHandType.TYPE_RIGHT_HAND);
        RightHand.EnableLog();
        RightHand.SetHandTransform(tHand, Module.eModuleAxis.AXIS_X, Module.eModuleAxis.AXIS_Z, Module.eModuleAxis.AXIS_Y);
        RightHand.SetFingerTransform(tThumb, tIndex, tMiddle, tRing, tPinky);

        //Needed to enable Captoglove sensor as Right Arm
        if (UseArm)
            ArmObject.GetComponent<MainRightArm>().enabled = true; 
        else
            ArmObject.GetComponent<MainRightArm>().enabled = false;

        //Handles objects color
        rCapsule = tCapsule.GetComponent<Renderer>();
        rButton = tButton.GetComponent<Renderer>();
        rLever = tLever.GetComponent<Renderer>();
        rLigth = tLigth.GetComponent<Renderer>();

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
        if (RightHand.IsHandClosed() && bCapsule)
        {                      
            CatchCapsule();
        }
        //Interaction with Lever
        else if (RightHand.IsHandClosed() && bLever)
        {
            CatchLever();
        }
        //Ineraction with light
        else if (RightHand.FingerGesture1() && bLight) 
		{
			rLigth.material.SetColor("_Color", Color.red);
		}
        //Ineraction with light
        else if (RightHand.FingerGesture2() && bLight)
		{
			rLigth.material.SetColor("_Color", Color.yellow);
		}
        //Ineraction with light
        else if (RightHand.FingerGesture3() && bLight)
		{
			rLigth.material.SetColor("_Color", Color.green);
		}
        //Ineraction with button
        else if (RightHand.IsSensorPressed() && bButton)
		{			
			PressButton(true);
		}
		else if (!RightHand.IsSensorPressed())
		{
            PressButton(false);
		}

    }

    private void CatchCapsule()
    {
        tCapsule.position = RightHand.GetMiddlePosition(); 
    }

    private void CatchLever()
    {
        Vector3 vHandPos = RightHand.GetHandPosition();

        if ((vHandPos.y - 2) > 7.5f &&
            (vHandPos.y - 2) < 12f)
        {
            tLever.position = new Vector3(tLever.position.x, vHandPos.y - 2, tLever.position.z);
        }
    }

    private void PressButton(bool b)
    {
        if (b)
            tButton.position = new Vector3(-11.88f, 9, 20.06f);
        else
            tButton.position = new Vector3(-11.88f, 9, 19.48f);
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.tag == "Capsule")
        {
            Debug.Log("Capsule OnTriggerEnter!!");
            rCapsule.material.SetColor("_Color", Color.green);
            bCapsule = true;
        }
        else if (other.tag == "Button")
        {
            Debug.Log("Button OnTriggerEnter!!");
            rButton.material.SetColor("_Color", Color.green);
            bButton = true; 
        }
        else if (other.tag == "Lever")
        {
            Debug.Log("Lever OnTriggerEnter!!");
            rLever.material.SetColor("_Color", Color.green);
            bLever = true;
        }
        else if (other.tag == "Light")
        {
            Debug.Log("Light OnTriggerEnter!!");
            bLight = true;           
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.tag == "Capsule")
        {
            rCapsule.material.SetColor("_Color", Color.red);
            bCapsule = false;
        }
        else if (other.tag == "Button")
        {            
            rButton.material.SetColor("_Color", Color.red);
            bButton = false;
        }
        else if (other.tag == "Lever")
        {            
            rLever.material.SetColor("_Color", Color.red);
            bLever = false;
        }
        else if (other.tag == "Light")
        {            
            bLight = false;
        }

    }

    void OnGUI()
    {
        /*
        if (RightHand.GetPropertiesRead())
        {
            GUI.Label(new Rect(0, 0, 200f, 200f), "Right Hand ready", style);
        }
        if (blights)
        {
            GUI.Label(new Rect(0, 40f, 200f, 200f), "Light box", style);
        }*/
    }

    private void OnDestroy()
    {
        if(RightHand!= null)
            RightHand.Stop();
    }

}
