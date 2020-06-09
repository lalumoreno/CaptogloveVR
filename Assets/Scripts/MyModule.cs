//CLEAN HEADERS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel; //needed for PropertyChangedEventArgs

using GSdkNet;
using GSdkNet.BLE;
using GSdkNet.Board;
using GSdkNet.Logging;
using GSdkNet.Peripheral;
using GSdkNet.BLE.Winapi;

//namespace Module
//{		
	public class MyModule : MonoBehaviour
	{
		public enum ModuleType
		{
			TYPE_RIGHT_HAND,
			TYPE_LEFT_HAND,
			TYPE_RIGHT_ARM,
			TYPE_LEFT_ARM
		}

		public enum ModuleAxis
		{
			AXIS_X,
			AXIS_Y,
			AXIS_Z
		}

		public enum ModuleMov
		{
			MOV_WIRST,
			MOV_ROLL,
			MOV_YAW
		}

		private int 		_nModuleID;
		private string 		_sModuleName; 
		private ModuleType	_enModuleType;
		private bool		_bIsAssigned; 
		private bool		_bIsInitialized;		
	
		protected Transform 	_model;
		
		private IPeripheralCentral		_Central;	 //CHECK IF NEED INITIALIZATION 
		protected IBoardPeripheral		_Peripheral;		
		protected BoardStreamEventArgs 	_tareadQuartEvent;
		protected BoardStreamEventArgs 	_sensorStateEvent;
		protected BoardStreamEventArgs  _linearAccEvent;

		//wirst
		protected ModuleAxis	_pitchAxis, _yawAxis, _rollAxis;
		protected float   		_x, _y, _z;
		
		//Fingers
		protected float[]	SensorMax; 
		protected float[]	SensorMin; 
		
		public bool sensorRead; 
		
		protected void InitModule(int nID, ModuleType enType){			
			SetModuleID(nID);			
			SetModuleType(enType);	
			SetIsInitialized(false);
			SetIsAssigned(true);
			
			//Default values 
			SensorMax = new float[10];
			SensorMin = new float[10];
			
			for(int i = 0; i<10; i++){
				SensorMax[i] = 0f;
				SensorMin[i] = 0f; 
			}
			
			sensorRead = false;
		}
		//DEFINE WICH ARE PRIVATE AND PUBLIC AND PROTECTED 
		//Set module ID
		private void SetModuleID(int nID){
			_nModuleID = nID;	
			SetModuleName();
		}		
		private int GetModuleID(){
			return _nModuleID; 			
		}
		
		//Set Module Name
		private void SetModuleName(){
			_sModuleName = "CaptoGlove"+_nModuleID.ToString();
		}		
		private string GetModuleName(){
			return _sModuleName;
		}
		
		//Set Module Type
		private void SetModuleType(ModuleType enType){
			_enModuleType = enType;
		}
		private ModuleType GetModuleType(){
			return _enModuleType;
		}
		
		//Set Assigned
		private void SetIsAssigned(bool b){
			_bIsAssigned = b;
		}
		private bool GetIsAssigned(){
			return _bIsAssigned;
		}
		
		//Set Initialized
		private void SetIsInitialized(bool b){
			_bIsInitialized = b; 
		}
		protected bool GetIsInitialized(){
			return _bIsInitialized;
		}
		
		//Set Model 
		public void SetTransform(Transform transform, ModuleAxis pitch, ModuleAxis yaw, ModuleAxis roll){
			_model 		= transform;	
			_pitchAxis  = pitch;
			_yawAxis  	= yaw;
			_rollAxis	= roll;
						
			_x = _model.localEulerAngles.x;
			_y = _model.localEulerAngles.y;
			_z = _model.localEulerAngles.z;
			
		}
		
		private Transform GetModel(){
			return _model;
		}
		
		public int Start(){
			
			if(_bIsAssigned){
				TraceDebug("Start, Looking for peripheral");			
 				
				var adapterScanner 	= new AdapterScanner();
				var adapter 		= adapterScanner.FindAdapter();
				var configurator 	= new Configurator(adapter);
		
				_Central = configurator.GetBoardCentral();
				_Central.PeripheralsChanged += Central_PeripheralsChanged; //If peripheral is detected 
				_Central.StartScan(new Dictionary<PeripheralScanFlag, object> {
					{ PeripheralScanFlag.ScanType, BleScanType.Balanced }
				});      	 
			
				SetIsInitialized(true); 
				
				return 0; 
			}
			else{
				TraceDebug("Module not created correctly");
				return -1;
			}
		}
		
		private async void ReadProperties(){
			
			SensorDescriptor 	sens = new SensorDescriptor();
			EmulationModes 		emu = new EmulationModes();
			StreamTimeslots 	st = new StreamTimeslots();			
			
			TraceDebug("----- Read Attributes -------"); 			
			TraceDebug("FW: " + _Peripheral.FirmwareVersion);
			//READ MORE ATTRIBUTES
						
			TraceDebug("----- Read Emulation Modes---");
			await _Peripheral.EmulationModes.ReadAsync();
			emu = _Peripheral.EmulationModes.Value; 
			TraceDebug (emu.ToString());
					
			TraceDebug("----- Read Timeslots --------");
			await _Peripheral.StreamTimeslots.ReadAsync();
			st = _Peripheral.StreamTimeslots.Value;
			TraceDebug(st.ToString());	

			if(_enModuleType == ModuleType.TYPE_LEFT_HAND || _enModuleType == ModuleType.TYPE_RIGHT_HAND){
				
				TraceDebug("----- Read sensors Calibration ------");

				for(int i = 0; i<10; i++){
				
					await _Peripheral.SensorDescriptors[i].ReadAsync(); 
					sens =  _Peripheral.SensorDescriptors[i].Value; 					
					TraceDebug("sensor "+i+sens.ToString());
					SensorMin[i] = sens.MinValue; 
					SensorMax[i] = sens.MaxValue;						
				}
			}

			sensorRead = true;
	}
		
		//This overwrites previous configurations 
		private void SetProperties(){
			
			StreamTimeslots st = new StreamTimeslots();
			
			TraceDebug("----- Calibration -----------");				
			_Peripheral.CalibrateGyroAsync();
			_Peripheral.TareAsync();
			_Peripheral.CommitChangesAsync();
			//VERIFY IF THIS CALIBRATION WORKS 
					
			TraceDebug("----- Set timeslot ----------");
			// without commit this changes are temporal					
			st.Set(6, BoardStreamType.TaredQuaternion); //Wirst
			st.Set(1, BoardStreamType.LinearAcceleration); //LinearAcc
		

		if (_enModuleType == ModuleType.TYPE_LEFT_HAND || _enModuleType == ModuleType.TYPE_RIGHT_HAND){
				st.Set(6, BoardStreamType.SensorsState);	//Fingers
			}

			_Peripheral.StreamTimeslots.WriteAsync(st);	
			//ADD MORE VALUES
				
			//SET EMULATION MODE 
			//SET SENSOR CALIBRATION? 
		}
			
		private async void Central_PeripheralsChanged(object sender, PeripheralsEventArgs e) {
			
			foreach (var peripheral in e.Inserted) {
				// Enumerate peripherals and run first connected
				try {
					var board = peripheral as IBoardPeripheral;
					TraceDebug("Trying to connect peripheral");
					TraceDebug("ID: " + board.Id);
					TraceDebug("Name: " + board.Name);
		
					//If it is the defined module
					if(board.Name == _sModuleName){
						_Peripheral = board;
						_Peripheral.PropertyChanged += Peripheral_PropertyChanged; //Set configurations
						_Peripheral.StreamReceived += Peripheral_StreamReceived;   //Read stream
						await _Peripheral.StartAsync();
					} 				
					return;
				} catch (Exception ex) {
					TraceDebug("Unable to start board " + ex.Message);
				}
			}
		
		}
		
		private void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e) {
		
			//TraceDebug("- Property changed: " + e.PropertyName.ToString());  
		 
			if (e.PropertyName == PeripheralProperty.Status) {
			
				TraceDebug("Board status: " + _Peripheral.Status.ToString());
			
				if (_Peripheral.Status == PeripheralStatus.Connected) {
									
					SetProperties();
					ReadProperties();					
					
				}
			}
		}

	private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e)
	{

		//TraceDebug("- Stream Received : " + e.StreamType.ToString());		
		//var args = e as BoardFloatValueEventArgs;

		if (e.StreamType == BoardStreamType.TaredQuaternion)
		{
			_tareadQuartEvent = e;
		}

		if (e.StreamType == BoardStreamType.SensorsState)
		{
			_sensorStateEvent = e;
		}
		//CAPTURE MORE VALUES 
		if (e.StreamType == BoardStreamType.LinearAcceleration)
		{
			_linearAccEvent = e; 
			//TraceDebug("Linear Acc" + args.Value.Z);
		}/*
		if (e.StreamType == BoardStreamType.GyroRate)
		{
			var args = e as BoardFloatVectorEventArgs;
			TraceDebug("GyroRate" + args.Value);
		}/*
		if (e.StreamType == BoardStreamType.RawAcceleration)
		{
			TraceDebug("RawAcceleration" + args.Value);
		}
		if (e.StreamType == BoardStreamType.TaredAltitude)
		{
			TraceDebug("TaredAltitude" + args.Value);
		}*/
	}
		public async void Stop() {
			
			TraceDebug("Stopping");
		
			if (_Peripheral != null) {
				TraceDebug("Stop Peripheral");
				_Peripheral.StreamReceived -= Peripheral_StreamReceived;
				_Peripheral.PropertyChanged -= Peripheral_PropertyChanged;
				await _Peripheral.StopAsync();
				_Peripheral.Dispose();
				_Peripheral = null;
			}
		
			if (_Central != null) {
				TraceDebug("Stop Central");
				_Central.StopScan();
				_Central.PeripheralsChanged -= Central_PeripheralsChanged;
				_Central = null;
			}
        
			TraceDebug("Stopped");
		}		
		
		//Utility
		protected void TraceDebug(string s){
			Debug.Log(_sModuleName + " >>> " + s);
		}

    } //Class MyModule
//} //namespace Module
