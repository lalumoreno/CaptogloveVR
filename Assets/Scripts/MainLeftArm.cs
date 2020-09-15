using UnityEngine;

public class MainLeftArm : MonoBehaviour
{
    public Transform tArm;
    private MyArm LeftArm = null;

    // Start is called before the first frame update
    void Start()
    {
        //Configuration for Captoglove sensor as Left Arm
        LeftArm = new MyArm(2469, MyArm.eArmType.TYPE_LEFT_FOREARM);
        LeftArm.EnableLog();
        LeftArm.SetArmTransform(tArm, Module.eModuleAxis.AXIS_Y, Module.eModuleAxis.AXIS_Z, Module.eModuleAxis.AXIS_X);
        LeftArm.SetInitialArmRot(180,0,90);
        LeftArm.Start();

    }

    // Update is called once per frame
    void Update()
    {
        //Enable simulation of arm movement
        LeftArm.MoveArm();
    }

    private void OnDestroy()
    {
        if(LeftArm!=null)
            LeftArm.Stop();
    }

}
