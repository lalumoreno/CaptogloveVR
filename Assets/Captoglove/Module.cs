using System;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

using GSdkNet;
using GSdkNet.BLE;
using GSdkNet.Board;
using GSdkNet.Peripheral;
using GSdkNet.BLE.Winapi;

/* 
     Class: Module
     Handle Captoglove module. Used by MyHand and MyArm
*/
public class Module : MonoBehaviour
{
    /* 
        Enum: peModuleType
        List of Captoglove module use mode:
    
        TYPE_RIGHT_HAND - As Right Hand sensor
        TYPE_LEFT_HAND - As Left Hand sensor
        TYPE_RIGHT_ARM - As Low Right Arm sensor
        TYPE_LEFT_ARM - As Low Left Arm sensor
    */
    protected enum peModuleType
    {
        TYPE_RIGHT_HAND,
        TYPE_LEFT_HAND,
        TYPE_RIGHT_ARM,
        TYPE_LEFT_ARM
    }

    /* 
        Enum: eModuleAxis
        List of axes:

        AXIS_X - X Axis 
        AXIS_Y - Y Axis
        AXIS_Z - Z Axis
    */
    public enum eModuleAxis
    {
        AXIS_X,
        AXIS_Y,
        AXIS_Z
    }

    private peModuleType _eModuleType;
    private int _nModuleID;
    private string _sModuleName;
    private bool _bModuleInitialized;
    private bool _bModuleStarted;
    private bool _bPropertiesRead;
    private bool _bLogEnabled;
    private IPeripheralCentral _IModuleCentral;
    private IBoardPeripheral pIModuleBoard;

    protected BoardStreamEventArgs psEventTaredQuart;
    protected BoardStreamEventArgs psEventSensorState;
    protected BoardStreamEventArgs psEventLinearAcceleration;

    protected float[] pfFingerSensorMaxValue;
    protected float[] pfFingerSensorMinValue;

    /* 
        Function: SetModuleType
        Saves Captoglove module use mode

        Parameters:
        enType - Captoglove module use mode
   
        Example:
        SetModuleType(Module.peModuleType.TYPE_RIGHT_HAND);
    */
    private void SetModuleType(peModuleType enType)
    {
        _eModuleType = enType;
    }
    /* 
    Function: GetModuleType
    Returns:
    _eModuleType - Captoglove module use mode
    */
    private peModuleType GetModuleType()
    {
        return _eModuleType;
    }
    /* 
    Function: SetModuleID
    Saves Captoglove module ID

    Parameters:
    nID - Captoglove module ID (4 digits number)

    Example:
    SetModuleID(2496);
*/
    private void SetModuleID(int nID)
    {
        _nModuleID = nID;
        SetModuleName();
    }
    /* 
        Function: GetModuleID
        Returns:
        _nModuleID - Captoglove module ID
    */
    private int GetModuleID()
    {
        return _nModuleID;
    }

    //Set SetModule Name
    private void SetModuleName()
    {
        _sModuleName = "CaptoGlove" + _nModuleID.ToString();
    }
    private string GetModuleName()
    {
        return _sModuleName;
    }




    /* 
       Function: SetModule
       Initializes variables for Captoglove module configuration
        
       Parameters:
       nID - Captoglove ID (4 digits number)
       etype - Captoglove use mode

       Example:
       SetModule(2496, Module.peModuleType.TYPE_RIGHT_HAND);
    */
    protected void SetModule(int nID, peModuleType eType)
    {
        SetModuleID(nID);
        SetModuleType(eType);
        SetIsInitialized(true);
        SetIsStarted(false);
        SetPropertiesRead(false);
        DisableLog();

        //Default values 
        pfFingerSensorMaxValue = new float[10];
        pfFingerSensorMinValue = new float[10];

        for (int i = 0; i < 10; i++)
        {
            pfFingerSensorMaxValue[i] = 0f;
            pfFingerSensorMinValue[i] = 0f;
        }
    }

    /* 
        Function: SetModuleID
        Save Captoglove module ID and create module name

        Parameters:
        nID - Captoglove ID (4 digits number)   

        Example:
        SetModuleID(2496);
    */
 

   

    //Set Assigned
    private void SetIsInitialized(bool b)
    {
        _bModuleInitialized = b;
    }
    protected bool GetIsInitialized()
    {
        return _bModuleInitialized;
    }

    //Set Initialized
    private void SetIsStarted(bool b)
    {
        _bModuleStarted = b;
    }
    protected bool GetIsStarted()
    {
        return _bModuleStarted;
    }

    private void SetPropertiesRead(bool b)
    {
        _bPropertiesRead = b;
    }

    protected bool GetPropertiesReady()
    {
        return _bPropertiesRead;
    }
    public void EnableLog()
    {
        _bLogEnabled = true;
    }

    private void DisableLog()
    {
        _bLogEnabled = false;
    }

    private bool GetIsLogEnabled()
    {
        return _bLogEnabled;
    }
    /*
     Function: Start
     Start Captoglove module and sensors

     Returns:
     0 - Success
     -1 - Error: Module not initialized
     */
    public int Start()
    {
        //TODO Add enable/disable debug 
        if (GetIsInitialized())
        {
            TraceLog("Start, Looking for peripheral");

            var adapterScanner = new AdapterScanner();
            var adapter = adapterScanner.FindAdapter();
            var configurator = new Configurator(adapter);

            _IModuleCentral = configurator.GetBoardCentral();
            _IModuleCentral.PeripheralsChanged += Central_PeripheralsChanged; //If peripheral is detected 
            _IModuleCentral.StartScan(new Dictionary<PeripheralScanFlag, object> {
                    { PeripheralScanFlag.ScanType, BleScanType.Balanced }
                });

            SetIsStarted(true);
        }
        else
        {
            TraceLog("Module not initialized correctly");
            return -1;
        }

        return 0;
    }

    private async void ReadProperties()
    {
        SensorDescriptor sensorDescriptor = new SensorDescriptor();
        EmulationModes emulationModes = new EmulationModes();
        StreamTimeslots streamTimeSlots = new StreamTimeslots();

        TraceLog("----- Read Attributes -------");
        TraceLog("FW: " + pIModuleBoard.FirmwareVersion);
        //TODO READ MORE ATTRIBUTES

        TraceLog("----- Read Emulation Modes---");
        await pIModuleBoard.EmulationModes.ReadAsync();
        emulationModes = pIModuleBoard.EmulationModes.Value;
        TraceLog(emulationModes.ToString());

        TraceLog("----- Read Timeslots --------");
        await pIModuleBoard.StreamTimeslots.ReadAsync();
        streamTimeSlots = pIModuleBoard.StreamTimeslots.Value;
        TraceLog(streamTimeSlots.ToString());

        if (_eModuleType == peModuleType.TYPE_LEFT_HAND || _eModuleType == peModuleType.TYPE_RIGHT_HAND)
        {
            TraceLog("----- Read sensors Calibration ------");

            for (int i = 0; i < 10; i++)
            {
                await pIModuleBoard.SensorDescriptors[i].ReadAsync();
                sensorDescriptor = pIModuleBoard.SensorDescriptors[i].Value;
                TraceLog("sensor " + i + sensorDescriptor.ToString());
                pfFingerSensorMinValue[i] = sensorDescriptor.MinValue;
                pfFingerSensorMaxValue[i] = sensorDescriptor.MaxValue;
            }
        }

        SetPropertiesRead(true);
    }

    /* Function: SetProperties
    Set Captoglove module properties: 
    1. Calibrate module
    2. Tare module
    3. Commit changes
    4. Set time slots
    
    Notes: 
    This function overwrites previous configurations 
    */

    private void SetProperties()
    {
        StreamTimeslots st = new StreamTimeslots();

        TraceLog("----- Calibration -----------");
        pIModuleBoard.CalibrateGyroAsync();
        pIModuleBoard.TareAsync();
        pIModuleBoard.CommitChangesAsync();
        //VERIFY IF THIS CALIBRATION WORKS 

        TraceLog("----- Set timeslot ----------");
        // without commit this changes are temporal					
        st.Set(6, BoardStreamType.TaredQuaternion); //Wirst
        st.Set(1, BoardStreamType.LinearAcceleration); //LinearAcc


        if (_eModuleType == peModuleType.TYPE_LEFT_HAND || _eModuleType == peModuleType.TYPE_RIGHT_HAND)
        {
            st.Set(6, BoardStreamType.SensorsState);    //Fingers
        }

        pIModuleBoard.StreamTimeslots.WriteAsync(st);
        //TODO ADD MORE VALUES

        //TODO SET EMULATION MODE 
        //TODO SET SENSOR CALIBRATION? 
    }

    /* Function: Central_PeripheralsChanged
     Detects Captoglove modules available and searches for module ID 
      
     Parameres: 
     sender - 
     e -   

     */
    private async void Central_PeripheralsChanged(object sender, PeripheralsEventArgs e)
    {
        foreach (var peripheral in e.Inserted)
        {
            // Enumerate peripherals and run first connected
            try
            {
                var board = peripheral as IBoardPeripheral;
                TraceLog("Trying to connect peripheral");
                TraceLog("ID: " + board.Id);
                TraceLog("Name: " + board.Name);

                //If module ID is found
                if (board.Name == _sModuleName)
                {
                    pIModuleBoard = board;
                    pIModuleBoard.PropertyChanged += Peripheral_PropertyChanged; //Set configurations
                    pIModuleBoard.StreamReceived += Peripheral_StreamReceived;   //Read stream
                    await pIModuleBoard.StartAsync();
                }
                return;
            }
            catch (Exception ex)
            {
                TraceLog("Unable to start board " + ex.Message);
            }
        }

    }

    /* Function: Peripheral_PropertyChanged
    Detects when Captoglove module is connected and set properties
     
    Parameres: 
    sender - 
    e - 
    */
    private void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //TraceLog("- Property changed: " + e.PropertyName.ToString());  

        if (e.PropertyName == PeripheralProperty.Status)
        {
            TraceLog("Board status: " + pIModuleBoard.Status.ToString());

            if (pIModuleBoard.Status == PeripheralStatus.Connected)
            {
                SetProperties();
                ReadProperties();
            }
        }
    }

    /* Function: Peripheral_StreamReceived
    Read continuously stream sent by Captoglove module
     
    Parameres: 
    sender - 
    e - 
    */
    private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e)
    {
        //TraceLog("- Stream Received : " + e.StreamType.ToString());		
        //var args = e as BoardFloatValueEventArgs;

        if (e.StreamType == BoardStreamType.TaredQuaternion)
        {
            psEventTaredQuart = e;
        }

        if (e.StreamType == BoardStreamType.SensorsState)
        {
            psEventSensorState = e;
        }
        //CAPTURE MORE VALUES 
        if (e.StreamType == BoardStreamType.LinearAcceleration)
        {
            psEventLinearAcceleration = e;
            //TraceLog("Linear Acc" + args.Value.Z);
        }
    }

    /* Function: Stop
    Stop communication with Captoglove module
     
    Note: 
    Call this function when application is stopped
    */
    public async void Stop()
    {
        TraceLog("Stopping");

        if (pIModuleBoard != null)
        {
            TraceLog("Stop Peripheral");
            pIModuleBoard.StreamReceived -= Peripheral_StreamReceived;
            pIModuleBoard.PropertyChanged -= Peripheral_PropertyChanged;
            await pIModuleBoard.StopAsync();
            pIModuleBoard.Dispose();
            pIModuleBoard = null;
        }

        if (_IModuleCentral != null)
        {
            TraceLog("Stop Central");
            _IModuleCentral.StopScan();
            _IModuleCentral.PeripheralsChanged -= Central_PeripheralsChanged;
            _IModuleCentral = null;
        }

        TraceLog("Stopped");
    }

    protected void TraceLog(string s)
    {
        if (GetIsLogEnabled())
            Debug.Log(_sModuleName + " >>> " + s);
    }

} //Class Module

