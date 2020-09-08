using UnityEngine;
using System.ComponentModel; //needed for PropertyChangedEventArgs
using System.Linq; //Needed for String List 

using GSdkNet.Board;
using System;

public class MyArm : Module
{
    private peModuleType _enArmType;

    private Transform model;
    private int BaseRotation;

    private float UnityMax, UnityMin;
    public float pitchA, pitchB, yawA, yawB;

    private eModuleAxis _ePitchAxis, _eYawAxis, _eRollAxis;

    private Transform _tArmTransform;
    private float pfWirstXAngle, pfWirstYAngle, pfWirstZAngle;
    //Constructor 
    public MyArm(int nID, bool rightArm)
    {

        _enArmType = peModuleType.TYPE_LEFT_ARM;

        if (rightArm)
        {
            _enArmType = peModuleType.TYPE_RIGHT_ARM;
        }

        SetModule(nID, _enArmType);

    }


    /* Function: SetArmTransform    
    Attaches Captoglove module movement to object transform.     

    Parameters:
    tHandObject - 3D Arm object
    ePitch - Object axis for pitch movement 
    eYaw   - Object axis for yaw movement 
    eRoll  - Object axis for roll movement 

    Returns: 
    0  - Success
    -1 - Failure

    Notes: 
    Place the object horizontally in the scene before assigning it in this function.
    */
    public int SetArmTransform(Transform tHandObject, eModuleAxis ePitch, eModuleAxis eYaw, eModuleAxis eRoll)
    {
        if (tHandObject == null)
            return -1;

        _tArmTransform = tHandObject;
        _ePitchAxis = ePitch;
        _eYawAxis = eYaw;
        _eRollAxis = eRoll;

        pfWirstXAngle = _tArmTransform.localEulerAngles.x;
        pfWirstYAngle = _tArmTransform.localEulerAngles.y;
        pfWirstZAngle = _tArmTransform.localEulerAngles.z;

        return 0;
    }
    public void Pitch(float UpRotation, float DownRotation)
    {
        UnityMax = UpRotation;
        UnityMin = DownRotation;

        pitchA = (UnityMax - UnityMin) / (0.5f - (-0.5f));
        pitchB = UnityMin - pitchA * (-0.5f);

        //TraceLog("_fPitchA = "+_fPitchA+" _fPitchB = " + _fPitchB); 
    }

    public void Yaw(float RightRotation, float LeftRotation)
    {
        UnityMax = LeftRotation;
        UnityMin = RightRotation;

        yawA = (UnityMax - UnityMin) / (0.5f - (-0.5f));
        yawB = UnityMin - yawA * (-0.5f);

        //TraceLog("_fYawA = "+_fYawA+" _fYawB = " + _fYawB);
    }

    //Wirst movement 
    //CHECK IF CAN BE DONE THE SAME FOR ARMS AND HANDS
    private void SetWirstNewAngle()
    {
        var args = psEventTaredQuart as BoardQuaternionEventArgs;
        float pitchAngle;
        float yawAngle;

        var args2 = psEventLinearAcceleration as BoardFloatVectorEventArgs;

        if (args != null)
        {
            //TraceLog("- Stream Received : " + psEventTaredQuart.StreamType.ToString());	

            float quaternionX = args.Value.X;
            float quaternionY = args.Value.Y;
            float quaternionZ = args.Value.Z;

            pitchAngle = quaternionX * pitchA + pitchB;
            yawAngle = quaternionY * yawA + yawB;

            AsignAngle2Axes(pitchAngle, yawAngle, 0);

            if (args2 != null)
            {
                float ZAcc = args2.Value.Z;

                if (ZAcc > 0.3f &&
                    (Mathf.Abs(quaternionX) < 0.05f) &&
                    (Mathf.Abs(quaternionY) < 0.05f) &&
                    (Mathf.Abs(quaternionZ) < 0.05f))
                {
                    TraceLog("Move arm backguard");
                }
            }
        }
    }

    private void AsignAngle2Axes(float pitchA, float yawA, float rollA)
    {
        switch (_ePitchAxis)
        {
            case eModuleAxis.AXIS_X:
                pfWirstXAngle = pitchA;
                break;
            case eModuleAxis.AXIS_Y:
                pfWirstYAngle = pitchA;
                break;
            case eModuleAxis.AXIS_Z:
                pfWirstZAngle = pitchA;
                break;
        }

        switch (_eYawAxis)
        {
            case eModuleAxis.AXIS_X:
                pfWirstXAngle = yawA;
                break;
            case eModuleAxis.AXIS_Y:
                pfWirstYAngle = yawA;
                break;
            case eModuleAxis.AXIS_Z:
                pfWirstZAngle = yawA;
                break;
        }

        switch (_eRollAxis)
        {
            case eModuleAxis.AXIS_X:
                //		pfWirstXAngle = _fRollA;
                break;
            case eModuleAxis.AXIS_Y:
                //		pfWirstYAngle = _fRollA;
                break;
            case eModuleAxis.AXIS_Z:
                //		pfWirstZAngle = _fRollA; 
                break;
        }
    }

    public void Wirst()
    {
        if (GetIsInitialized())
        {
            SetWirstNewAngle();
            SetLinearPosition();
        }

        _tArmTransform.localEulerAngles = new Vector3(pfWirstXAngle, pfWirstYAngle, pfWirstZAngle);
    }

    private void SetLinearPosition()
    {



    }

    //Utility
    private static string FloatsToString(float[] value)
    {
        string result = "";
        var index = 0;
        foreach (var element in value)
        {
            if (index != 0)
            {
                result += ", ";
            }
            result += element.ToString();
            index += 1;
        }
        return result;
    }

} //Class MyHand
