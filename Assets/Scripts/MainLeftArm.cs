using UnityEngine;
using GITEICaptoglove;

public class MainLeftArm : MonoBehaviour
{
    private MyArm LeftArm = null;
    private GUIStyle style;

    // Start is called before the first frame update
    void Start()
    {
        //Configuration for Captoglove sensor as Left Arm
        LeftArm = new MyArm(2469, MyArm.ArmType.TYPE_LEFT_FOREARM);
        LeftArm.SetLogEnabled(true);
        LeftArm.SetArmObject(transform);
        LeftArm.SetInitialRot(180,0,90);

        //Messages in display
        style = new GUIStyle();
        style.normal.textColor = Color.black;

        LeftArm.Start();

    }

    // Update is called once per frame
    void Update()
    {
        //Enable simulation of arm movement
        LeftArm.MoveArm();
    }

    void OnGUI()
    {
        if (LeftArm.GetPropertiesRead())
        {
            GUI.Label(new Rect(0, 20, 200f, 200f), "Left Arm ready", style);
        }
    }

    private void OnDestroy()
    {
        if(LeftArm!=null)
            LeftArm.Stop();
    }

}
