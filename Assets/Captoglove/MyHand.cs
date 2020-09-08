using UnityEngine;
using GSdkNet.Board;

public class MyHand : Module
{
    public enum eHandType
    {
        TYPE_RIGHT_HAND,
        TYPE_LEFT_HAND
    }

    private eHandType _eHandType;

    private eModuleAxis _ePitchAxis, _eYawAxis, _eRollAxis;

    private Transform[] _tFingers;
    private Transform[] child1;
    private Transform[] child2;

    //Dynamic size
    private Transform[] _tThumbChilds;
    private Transform[] _tIndexChilds;
    private Transform[] _tMiddleChilds;
    private Transform[] _tRingChilds;
    private Transform[] _tPinkyChilds;

    private eModuleAxis _eFingerAxis;
    private Vector3[] _fFingerAngle;
    private Vector3[] _prevLocalEuler;
    private Vector3[] _fFirstPhalangeAngle;
    private Vector3[] _fSecPhalangeAngle;

    private int _nThumbPos, _nIndexPos, _nMiddlePos, _nRingPos, _nPinkyPos;
    private int _nPressurePos;
    private Transform _tHandTransform;
    private int _nVibrationDegree;
    private bool _bFingerAB;
    private float pfWirstXAngle, pfWirstYAngle, pfWirstZAngle;

    private float[] sensorVal;
    private float[] a;
    private float[] a1;
    private float[] a2;
    private float[] b;
    private float[] b1;
    private float[] b2;

    private float[] _fFingerMaxRotation;
    private float[] _fFingerMinRotation;

    private float[] _fFirstPhalangeMaxRotation;
    private float[] _fSecPhalangeMaxRotation;

    private float[] _fFirstPhalangeMinRotation;
    private float[] _fSecPhalangeMinRotation;
    private float _fPitchA, _fPitchB, _fYawA, _fYawB, _fRollA, _fRollB;    

    private float[] triggers;

    //Constructor 
    public MyHand(int nID, eHandType _eType)
    {
        _eHandType = _eType;

        if (_eHandType == eHandType.TYPE_RIGHT_HAND)
        {
            SetModule(nID, Module.peModuleType.TYPE_RIGHT_HAND);
            SetFingerSensors(1, 3, 5, 7, 9, 2);            
        }
        else
        {
            SetModule(nID, Module.peModuleType.TYPE_LEFT_HAND);
            SetFingerSensors(10, 8, 6, 3, 2, 9);          
        }

        a = new float[10];
        a1 = new float[10];
        a2 = new float[10];
        b = new float[10];
        b1 = new float[10];
        b2 = new float[10];

        _fFingerAngle = new Vector3[10];
        _fFirstPhalangeAngle = new Vector3[10];
        _fSecPhalangeAngle = new Vector3[10];
        _prevLocalEuler = new Vector3[10];

        _fFingerMaxRotation = new float[10];
        _fFirstPhalangeMaxRotation = new float[10];
        _fSecPhalangeMaxRotation = new float[10];
        _fFingerMinRotation = new float[10];
        _fFirstPhalangeMinRotation = new float[10];
        _fSecPhalangeMinRotation = new float[10];
        triggers = new float[10];
        sensorVal = new float[10];

        _tFingers = new Transform[10];
        child1 = new Transform[10];
        child2 = new Transform[10];

        for (int i = 0; i < 10; i++)
        {
            a[i] = 0f;
            a1[i] = 0f;
            a2[i] = 0f;
            b[i] = 0f;
            b1[i] = 0f;
            b2[i] = 0f;

            _fFingerAngle[i] = new UnityEngine.Vector3(0, 0, 0);
            _fFirstPhalangeAngle[i] = new UnityEngine.Vector3(0, 0, 0);
            _fSecPhalangeAngle[i] = new UnityEngine.Vector3(0, 0, 0);
            _prevLocalEuler[i] = _fFingerAngle[i];
            _fFingerMaxRotation[i] = 0f;
            _fFirstPhalangeMaxRotation[i] = 0f;
            _fSecPhalangeMaxRotation[i] = 0f;
            _fFingerMinRotation[i] = 0f;
            _fFirstPhalangeMinRotation[i] = 0f;
            _fSecPhalangeMinRotation[i] = 0f;
            triggers[i] = 0f;
            sensorVal[i] = 0f;
        }

        _nVibrationDegree = 0;
        _bFingerAB = false;

        SetDefaultValues();
    }

    /* Function: SetDefaultValues
    Set initial rotation of Hand and Fingers transform.    

    Note: 
    This values are valid only for Hand model given with libraries
    */

    private void SetDefaultValues()
    {
        if (_eHandType == eHandType.TYPE_RIGHT_HAND)
        {
            SetPitchRotation(90, -90);
            SetYawRotation(90, -90);
            SetRollRotation(-180, 180);

            SetThumbRotation(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            SetIndexRotation(-23.606f, -80, 5.069f, -75, 2.359f, -75);
            SetMiddleRotation(-26.575f, -80, 10.864f, -75, -3.127f, -75);
            SetRingRotation(-27.302f, -80, 11.405f, -75, -1.038f, -75);
            SetPinkyRotation(-24.763f, -80, 6.326f, -75, 5.373f, -75);
            SetVibrationLevel(2);
        }
        else
        {
            SetPitchRotation(-90, 90);
            SetYawRotation(-90, 90);
            SetRollRotation(-180, 180);

            SetThumbRotation(12.681f, 60, -0.992f, 50, 6.269001f, 50);
            SetIndexRotation(21.155f, 80, -5.408f, 75, -2.203f, 75);
            SetMiddleRotation(24.201f, 80, -10.915f, 75, 3.174f, 75);
            SetRingRotation(24.854f, 80, -10.759f, 75, 0.541f, 75);
            SetPinkyRotation(22.229f, 80, -5.971f, 75, -5.211f, 75);
            SetVibrationLevel(2);
        }
    }

    //This one is not needed
    internal void PressButton(Transform button, UnityEngine.Vector3 buttonPos)
    {
        button.localPosition = buttonPos;
        //Movent of fingers tIndexObj and tThumbObj together 
    }


    internal void CatchObject(Transform myObject)
    {
        myObject.position = _tFingers[_nMiddlePos].position;
    }

    //DEFINE WHICH ARE PRIVATE AND PUBLIC 		
    private void SetVibrationLevel(int nDegrees)
    {
        _nVibrationDegree = nDegrees;
    }


    /* Function: SetHandTransform
    Attaches Captoglove module movement to object transform.     

    Parameters:
    tHandObj - 3D Hand object
    ePitchAxis - Object axis for pitch movement 
    eYawAxis   - Object axis for yaw movement 
    eRollAxis  - Object axis for roll movement 

    Returns: 
     0 - Success
    -1 - Module not initialized
    -2 - Object error

    Notes: 
    Place the object horizontally in the scene before assigning it in this function.
    */
    public int SetHandTransform(Transform tHandObj, eModuleAxis ePitchAxis, eModuleAxis eYawAxis, eModuleAxis eRollAxis)
    {
        if (!GetIsInitialized())
            return -1;

        if (tHandObj == null)
            return -2;

        _tHandTransform = tHandObj;
        _ePitchAxis = ePitchAxis;
        _eYawAxis = eYawAxis;
        _eRollAxis = eRollAxis;

        pfWirstXAngle = _tHandTransform.localEulerAngles.x;
        pfWirstYAngle = _tHandTransform.localEulerAngles.y;
        pfWirstZAngle = _tHandTransform.localEulerAngles.z;

        return 0;
    }

    /* Function: SetFingerTransform
    Attaches Captoglove sensors to objects transform. 

    Parameters:
    tThumbObj  - 3D SetThumbRotation finger object
    tIndexObj  - 3D SetIndexRotation finger object
    tMiddleObj - 3D SetMiddleRotation finger object
    tRingObj   - 3D SetRingRotation finger object
    tPinkyObj  - 3D SetPinkyRotation finger object
    eMovementAxis - Fingers axis for movement
    nPhalangesNum - Number of finger phalanges in object (0 to 3)

    Returns: 
     0 - Success
    -1 - Module not initialized
    -2 - Object error
    -3 - Phalanges number error

    Notes: 
    Place the objects horizontally in the scene before assigning them in this function.    
*/
    public int SetFingerTransform(Transform tThumbObj, Transform tIndexObj, Transform tMiddleObj,
                                   Transform tRingObj, Transform tPinkyObj, eModuleAxis eMovementAxis,
                                   int nPhalangesNum)
    {
        if (!GetIsInitialized())
            return -1;

        //TODO: CHECK IF CAN BE DONE AUTOMATICALLY READING CHILDS FROM HAND TRANFORM
        if (tThumbObj == null ||
            tIndexObj == null ||
            tMiddleObj == null ||
            tRingObj == null ||
            tPinkyObj == null)
            return -2;

        if (nPhalangesNum < 0 || nPhalangesNum > 2)
            return -3;

        _tFingers[_nThumbPos] = tThumbObj;
        _tFingers[_nIndexPos] = tIndexObj;
        _tFingers[_nMiddlePos] = tMiddleObj;
        _tFingers[_nRingPos] = tRingObj;
        _tFingers[_nPinkyPos] = tPinkyObj;

        for (int i = 0; i < 10; i++)
        {
            if (_tFingers[i] != null)
                _fFingerAngle[i] = _tFingers[i].localEulerAngles;
        }

        _eFingerAxis = eMovementAxis;

        //TODO EVALUATE BEHAVIOUR IF NO CHILD IS FOUND AND REMOVE nPhalangesNum
        if (nPhalangesNum > 1)
        {
            _tThumbChilds = _tFingers[_nThumbPos].GetComponentsInChildren<Transform>();
            _tIndexChilds = _tFingers[_nIndexPos].GetComponentsInChildren<Transform>();
            _tMiddleChilds = _tFingers[_nMiddlePos].GetComponentsInChildren<Transform>();
            _tRingChilds = _tFingers[_nRingPos].GetComponentsInChildren<Transform>();
            _tPinkyChilds = _tFingers[_nPinkyPos].GetComponentsInChildren<Transform>();

            _fFirstPhalangeAngle[_nThumbPos] = _tThumbChilds[1].localEulerAngles;
            _fSecPhalangeAngle[_nThumbPos] = _tThumbChilds[2].localEulerAngles;

            _fFirstPhalangeAngle[_nIndexPos] = _tIndexChilds[1].localEulerAngles;
            _fSecPhalangeAngle[_nIndexPos] = _tIndexChilds[2].localEulerAngles;

            _fFirstPhalangeAngle[_nMiddlePos] = _tMiddleChilds[1].localEulerAngles;
            _fSecPhalangeAngle[_nMiddlePos] = _tMiddleChilds[2].localEulerAngles;

            _fFirstPhalangeAngle[_nRingPos] = _tRingChilds[1].localEulerAngles;
            _fSecPhalangeAngle[_nRingPos] = _tRingChilds[2].localEulerAngles;

            _fFirstPhalangeAngle[_nPinkyPos] = _tPinkyChilds[1].localEulerAngles;
            _fSecPhalangeAngle[_nPinkyPos] = _tPinkyChilds[2].localEulerAngles;
        }

        return 0;
    }

    private void SetPitchRotation(float fMaxUpRotation, float fMaxDownRotation)
    {
        _fPitchA = (fMaxUpRotation - fMaxDownRotation) / (0.5f - (-0.5f));
        _fPitchB = fMaxDownRotation - _fPitchA * (-0.5f);

        //TraceLog("_fPitchA = "+_fPitchA+" _fPitchB = " + _fPitchB); 
    }
    private void SetYawRotation(float fMaxRightRotation, float fMaxLeftRotation)
    {
        _fYawA = (fMaxLeftRotation - fMaxRightRotation) / (0.5f - (-0.5f));
        _fYawB = fMaxRightRotation - _fYawA * (-0.5f);

        //TraceLog("_fYawA = "+_fYawA+" _fYawB = " + _fYawB); 
    }
    private void SetRollRotation(float fMaxRightRotation, float fMaxLeftRotation)
    {
        _fRollA = (fMaxLeftRotation - fMaxRightRotation) / (1f - (-1f));
        _fRollB = fMaxRightRotation - _fRollA * (-1f);

        //TraceLog("_fRollA = "+_fRollA+" _fRollB = " + _fRollB); 
    }

    //TODO SEPARATE FUNCTIONS FOR PHALANGES
    private void SetThumbRotation(float fMinRotation, float fMaxRotation,
                                 float fMinRotation1, float fMaxRotation1,
                                 float fMinRotation2, float fMaxRotation2)
    {
        _fFingerMinRotation[_nThumbPos] = fMinRotation;
        _fFingerMaxRotation[_nThumbPos] = fMaxRotation;

        _fFirstPhalangeMinRotation[_nThumbPos] = fMinRotation1;
        _fFirstPhalangeMaxRotation[_nThumbPos] = fMaxRotation1;

        _fSecPhalangeMinRotation[_nThumbPos] = fMinRotation2;
        _fSecPhalangeMaxRotation[_nThumbPos] = fMaxRotation2;
    }

    private void SetIndexRotation(float fMinRotation, float fMaxRotation,
                                 float fMinRotation1, float fMaxRotation1,
                                 float fMinRotation2, float fMaxRotation2)
    {
        _fFingerMinRotation[_nIndexPos] = fMinRotation;
        _fFingerMaxRotation[_nIndexPos] = fMaxRotation;

        _fFirstPhalangeMinRotation[_nIndexPos] = fMinRotation1;
        _fFirstPhalangeMaxRotation[_nIndexPos] = fMaxRotation1;

        _fSecPhalangeMinRotation[_nIndexPos] = fMinRotation2;
        _fSecPhalangeMaxRotation[_nIndexPos] = fMaxRotation2;
    }
    private void SetMiddleRotation(float fMinRotation, float fMaxRotation,
                                  float fMinRotation1, float fMaxRotation1,
                                  float fMinRotation2, float fMaxRotation2)
    {
        _fFingerMinRotation[_nMiddlePos] = fMinRotation;
        _fFingerMaxRotation[_nMiddlePos] = fMaxRotation;

        _fFirstPhalangeMinRotation[_nMiddlePos] = fMinRotation1;
        _fFirstPhalangeMaxRotation[_nMiddlePos] = fMaxRotation1;

        _fSecPhalangeMinRotation[_nMiddlePos] = fMinRotation2;
        _fSecPhalangeMaxRotation[_nMiddlePos] = fMaxRotation2;
    }
    private void SetRingRotation(float fMinRotation, float fMaxRotation,
                                float fMinRotation1, float fMaxRotation1,
                                float fMinRotation2, float fMaxRotation2)
    {
        _fFingerMinRotation[_nRingPos] = fMinRotation;
        _fFingerMaxRotation[_nRingPos] = fMaxRotation;

        _fFirstPhalangeMinRotation[_nRingPos] = fMinRotation1;
        _fFirstPhalangeMaxRotation[_nRingPos] = fMaxRotation1;

        _fSecPhalangeMinRotation[_nRingPos] = fMinRotation2;
        _fSecPhalangeMaxRotation[_nRingPos] = fMaxRotation2;
    }
    private void SetPinkyRotation(float fMinRotation, float fMaxRotation,
                                 float fMinRotation1, float fMaxRotation1,
                                 float fMinRotation2, float fMaxRotation2)
    {
        _fFingerMinRotation[_nPinkyPos] = fMinRotation;
        _fFingerMaxRotation[_nPinkyPos] = fMaxRotation;

        _fFirstPhalangeMinRotation[_nPinkyPos] = fMinRotation1;
        _fFirstPhalangeMaxRotation[_nPinkyPos] = fMaxRotation1;

        _fSecPhalangeMinRotation[_nPinkyPos] = fMinRotation2;
        _fSecPhalangeMaxRotation[_nPinkyPos] = fMaxRotation2;
    }

    //Wirst movement 
    //CHECK IF CAN BE DONE THE SAME FOR ARMS AND HANDS
    private void SetWirstNewAngle()
    {
        var args = psEventTaredQuart as BoardQuaternionEventArgs;
        float pitchAngle;
        float yawAngle;
        float rollAngle;

        if (args != null)
        {
            //TraceLog("- Stream Received : " + psEventTaredQuart.StreamType.ToString());	

            float quaternionX = args.Value.X;
            float quaternionY = args.Value.Y;
            float quaternionZ = args.Value.Z;

            pitchAngle = quaternionX * _fPitchA + _fPitchB;
            yawAngle = quaternionY * _fYawA + _fYawB;
            rollAngle = quaternionZ * _fRollA + _fRollB;

            /*
            //SetPitchRotation when hand is upside down TODO IMPROVE THIS MOVEMENT 
            if((_eType == ModuleType.TYPE_LEFT_HAND &&	quaternionZ>0.9) ||
                (_eType == ModuleType.TYPE_RIGHT_HAND && quaternionZ<-0.9)) //Boca arriba
            {
                //	pitchAngle = -pitchAngle;
                yawAngle  = -yawAngle;
                AsignAngle2Axes(yawAngle, pitchAngle, rollAngle);
            }
            else*/
            {
                AsignAngle2Axes(pitchAngle, yawAngle, rollAngle);
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
                pfWirstXAngle = rollA;
                break;
            case eModuleAxis.AXIS_Y:
                pfWirstYAngle = rollA;
                break;
            case eModuleAxis.AXIS_Z:
                pfWirstZAngle = rollA;
                break;
        }
    }

    public void Wirst()
    {
        if (GetIsStarted())
        {
            SetWirstNewAngle();
        }

        _tHandTransform.localEulerAngles = new Vector3(pfWirstXAngle, pfWirstYAngle, pfWirstZAngle);
    }
    private void SetFingerSensors(int nThumbSensor, int nIndexSensor, int nMiddleSensormiddP, int nRingSensor, int nPinkySensor,
                                   int nPressureSensor)
    {
        //TODO Validate integers and make public
        //TODO verify if it is needed to reduce one in array position 

        //ArrayPos = SensorPos - 1 
        _nThumbPos = nThumbSensor - 1;
        _nIndexPos = nIndexSensor - 1;
        _nMiddlePos = nMiddleSensormiddP - 1;
        _nRingPos = nRingSensor - 1;
        _nPinkyPos = nPinkySensor - 1;
        _nPressurePos = nPressureSensor - 1;
    }

    private void SetFingersAB()
    {
        //TraceLog("Get AB ");
        float num = 0f;

        for (int i = 0; i < 10; i++)
        {
            num = pfFingerSensorMinValue[i] - pfFingerSensorMaxValue[i];

            if (num != 0f)
            {
                a[i] = (_fFingerMaxRotation[i] - _fFingerMinRotation[i]) / num;
                a1[i] = (_fFirstPhalangeMaxRotation[i] - _fFirstPhalangeMinRotation[i]) / num;
                a2[i] = (_fSecPhalangeMaxRotation[i] - _fSecPhalangeMinRotation[i]) / num;
            }

            b[i] = _fFingerMinRotation[i] - (a[i] * pfFingerSensorMaxValue[i]);
            b1[i] = _fFirstPhalangeMinRotation[i] - (a1[i] * pfFingerSensorMaxValue[i]);
            b2[i] = _fSecPhalangeMinRotation[i] - (a2[i] * pfFingerSensorMaxValue[i]);
        }

        //A and B are set correctly after the properties are read
        if (GetPropertiesReady())
            _bFingerAB = true;
    }

    //Fingers movement	
    private void SetFingersNewAngle()
    {
        var args = psEventSensorState as BoardFloatSequenceEventArgs;
        float temp;

        if (args != null)
        {

            //CALCULATE JUST ONCE 
            if (_bFingerAB == false)
            {
                SetFingersAB();
                for (int i = 0; i < 10; i++)
                {
                    triggers[i] = (pfFingerSensorMaxValue[i] - pfFingerSensorMinValue[i]) / 3;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                sensorVal[i] = args.Value[i];
                temp = sensorVal[i] * a[i] + b[i];

                //Avoid shaking
                if (Mathf.Abs(temp - _fFingerAngle[i].x) > _nVibrationDegree)
                {
                    _fFingerAngle[i].x = temp;
                    _fFirstPhalangeAngle[i].x = args.Value[i] * a1[i] + b1[i];
                    _fSecPhalangeAngle[i].x = args.Value[i] * a2[i] + b2[i];
                }

                if (Mathf.Abs(_fFingerAngle[i].x) < Mathf.Abs(_fFingerMinRotation[i]))
                {
                    _fFingerAngle[i].x = _fFingerMinRotation[i];
                    _fFirstPhalangeAngle[i].x = _fFirstPhalangeMinRotation[i];
                    _fSecPhalangeAngle[i].x = _fSecPhalangeMinRotation[i];
                }
            }
        }
    }

    public void Fingers()
    {

        if (GetIsStarted())
        {

            SetFingersNewAngle();

            for (int i = 0; i < 10; i++)
            {
                if (_tFingers[i] != null)
                {
                    _tFingers[i].localEulerAngles = _fFingerAngle[i];
                }
            }

            _tThumbChilds[1].localEulerAngles = _fFirstPhalangeAngle[_nThumbPos];
            _tThumbChilds[2].localEulerAngles = _fSecPhalangeAngle[_nThumbPos];

            _tIndexChilds[1].localEulerAngles = _fFirstPhalangeAngle[_nIndexPos];
            _tIndexChilds[2].localEulerAngles = _fSecPhalangeAngle[_nIndexPos];

            _tMiddleChilds[1].localEulerAngles = _fFirstPhalangeAngle[_nMiddlePos];
            _tMiddleChilds[2].localEulerAngles = _fSecPhalangeAngle[_nMiddlePos];

            _tRingChilds[1].localEulerAngles = _fFirstPhalangeAngle[_nRingPos];
            _tRingChilds[2].localEulerAngles = _fSecPhalangeAngle[_nRingPos];

            _tPinkyChilds[1].localEulerAngles = _fFirstPhalangeAngle[_nPinkyPos];
            _tPinkyChilds[2].localEulerAngles = _fSecPhalangeAngle[_nPinkyPos];

        } //Initialized

    }

    //No model is needed
    public void FingersAR()
    {
        if (GetIsStarted())
        {
            SetFingersNewAngle();

        } //Initialized

    }

    public bool isPressed()
    {
        bool bRet = false;

        if (GetIsStarted())
        {
            if (sensorVal[_nPressurePos] > triggers[_nPressurePos])
                bRet = true;
        }

        return bRet;
    }

    public bool isCatch()
    {
        bool bRet = false;

        if (GetIsStarted())
        {
            if (sensorVal[_nIndexPos] < triggers[_nIndexPos] &&
                sensorVal[_nMiddlePos] < triggers[_nMiddlePos] &&
                sensorVal[_nRingPos] < triggers[_nRingPos] &&
                sensorVal[_nPinkyPos] < triggers[_nPinkyPos])
            {

                bRet = true;
            }
        }

        return bRet;
    }

    public bool isNumber1()
    {
        bool bRet = false;

        if (GetIsStarted())
        {
            if (sensorVal[_nIndexPos] > triggers[_nIndexPos] &&
                sensorVal[_nMiddlePos] < triggers[_nMiddlePos] &&
                sensorVal[_nRingPos] < triggers[_nRingPos] &&
                sensorVal[_nPinkyPos] < triggers[_nPinkyPos])
            {

                bRet = true;
            }
        }

        return bRet;
    }

    public bool isNumber2()
    {
        bool bRet = false;

        if (GetIsStarted())
        {
            if (sensorVal[_nIndexPos] > triggers[_nIndexPos] &&
                sensorVal[_nMiddlePos] > triggers[_nMiddlePos] &&
                sensorVal[_nRingPos] < triggers[_nRingPos] &&
                sensorVal[_nPinkyPos] < triggers[_nPinkyPos])
            {
                bRet = true;
            }
        }

        return bRet;

    }
    public bool isNumber3()
    {
        bool bRet = false;

        if (GetIsStarted())
        {
            if (sensorVal[_nIndexPos] > triggers[_nIndexPos] &&
                sensorVal[_nMiddlePos] > triggers[_nMiddlePos] &&
                sensorVal[_nRingPos] > triggers[_nRingPos] &&
                sensorVal[_nPinkyPos] < triggers[_nPinkyPos])
            {
                bRet = true;
            }
        }

        return bRet;

    }
} //Class MyHand
