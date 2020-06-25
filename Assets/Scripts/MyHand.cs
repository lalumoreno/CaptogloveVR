using UnityEngine;
using System.ComponentModel; //needed for PropertyChangedEventArgs

using GSdkNet.Board;
using System.Numerics;
using System;

//namespace MyHand
//{	
public class MyHand: MyModule
	{
		private ModuleType	_enHandType; 
		
		private Transform[] _modelFingers;
		private Transform[] child1;
		private Transform[] child2;
		private Transform[] thmbChild;		
		private Transform[] idxChild;
		private Transform[] middChild;
		private Transform[] ringChild;
		private Transform[] pinkyChild;
		
		private ModuleAxis	_fingerAxis;			
		private UnityEngine.Vector3[]	_localEuler; 
		private UnityEngine.Vector3[] _prevLocalEuler;
		private UnityEngine.Vector3[] _localEuler1;
		private UnityEngine.Vector3[] _localEuler2;
		
		private int nThumbPos, nIndexPos, nMiddlePos, nRingPos, nPinkyPos;
		private int nPreassurePos; 
		
		private int shakeDegree; 
	
		private float[] sensorVal;
		private float[]	a;
		private float[]	a1;
		private float[]	a2;
		private float[]	b;
		private float[]	b1;
		private float[]	b2;
		
		private float UnityMax, UnityMin;
		private float[] UnityMaxFinger;
		private float[] UnityMaxFinger1;
		private float[] UnityMaxFinger2;
		private float[]	UnityMinFinger;
		private float[]	UnityMinFinger1;
		private float[]	UnityMinFinger2;
		public float pitchA, pitchB, yawA, yawB, rollA, rollB ; 
		
		private bool abSet; 
		
		private float[] triggers;

	//Constructor 
	public MyHand(int nID, bool rightHand){

			_enHandType = ModuleType.TYPE_LEFT_HAND; 
			
			if (rightHand){
				_enHandType = ModuleType.TYPE_RIGHT_HAND; 
			}
				
			InitModule(nID, _enHandType); 	 	
			
			//Default values 
			if(_enHandType == ModuleType.TYPE_RIGHT_HAND){
				
				//Sensor pos - 1  = Array pos
				nThumbPos 		= 1-1; 
				nPreassurePos 	= 2-1;
				nIndexPos 		= 3-1; 
				nMiddlePos 		= 5-1;
				nRingPos 		= 7-1; 
				nPinkyPos 		= 9-1; 				 
			}
			else {				
				nThumbPos 		= 10-1; 
				nPreassurePos 	= 9-1;
				nIndexPos 		= 8-1; 
				nMiddlePos 		= 6-1;
				nRingPos 		= 3-1; 
				nPinkyPos 		= 2-1; 
			}
			
			a 	= new float[10];
			a1 	= new float[10];
			a2 	= new float[10];
			b 	= new float[10];			
			b1 	= new float[10];			
			b2	= new float[10];			
			
			_localEuler = new UnityEngine.Vector3[10];
			_localEuler1 = new UnityEngine.Vector3[10];
			_localEuler2 = new UnityEngine.Vector3[10];
			_prevLocalEuler = new UnityEngine.Vector3[10];
			
			UnityMaxFinger = new float[10];
			UnityMaxFinger1 = new float[10];
			UnityMaxFinger2 = new float[10];
			UnityMinFinger = new float[10];
			UnityMinFinger1 = new float[10];
			UnityMinFinger2 = new float[10];
			triggers = new float[10];
			sensorVal = new float[10];
			
			_modelFingers = new Transform[10];
			child1 = new Transform[10];
			child2 = new Transform[10];
			
			for(int i = 0; i<10; i++){
				a[i] 	=0f;
				a1[i] 	=0f;
				a2[i] 	=0f;
				b[i] 	=0f;
				b1[i] 	=0f;
				b2[i] 	=0f;
				
				_localEuler[i] = new UnityEngine.Vector3(0,0,0);
				_localEuler1[i] = new UnityEngine.Vector3(0,0,0);
				_localEuler2[i] = new UnityEngine.Vector3(0,0,0);
				_prevLocalEuler[i] = _localEuler[i];
				UnityMaxFinger[i] =0f;
				UnityMaxFinger1[i] =0f;
				UnityMaxFinger2[i] =0f;
				UnityMinFinger[i] =0f;
				UnityMinFinger1[i] =0f;
				UnityMinFinger2[i] =0f;
				triggers[i]=0f;
				sensorVal[i] = 0f;
			}
			
			shakeDegree = 0;
			abSet = false;
		}

    internal void PressButton(Transform button, UnityEngine.Vector3 buttonPos)
    {
		button.localPosition = buttonPos;
		//Movent of fingers index and thumb together 
    }


    internal void CatchObject(Transform myObject)
    {
		myObject.position = _modelFingers[nMiddlePos].position;
    }

	//DEFINE WHICH ARE PRIVATE AND PUBLIC 		
	public void setShakeDegree(int dg){
			shakeDegree = dg; 
		}
		//CHECK IF CAN BE DONDE READING CHILDS FROM HAND TRANFORM
		//CHEKC IF GET IS NEEDED 
		public void SetFingerModel(Transform thumb, Transform index, Transform middle, Transform ring, Transform pinky, ModuleAxis axis, int falanges){
			
			_modelFingers[nThumbPos] 	= thumb;
			_modelFingers[nIndexPos] 	= index;
			_modelFingers[nMiddlePos]	= middle;
			_modelFingers[nRingPos] 	= ring;
			_modelFingers[nPinkyPos] 	= pinky;		
			
			_fingerAxis		= axis;

			for(int i = 0 ; i<10 ; i++){
				if(_modelFingers[i] != null)
					_localEuler[i] 	= _modelFingers[i].localEulerAngles;			
			}			
			
			if (falanges > 1){
			
				thmbChild  = _modelFingers[nThumbPos].GetComponentsInChildren<Transform>();
				idxChild   = _modelFingers[nIndexPos].GetComponentsInChildren<Transform>();
				middChild  = _modelFingers[nMiddlePos].GetComponentsInChildren<Transform>();
				ringChild  = _modelFingers[nRingPos].GetComponentsInChildren<Transform>();
				pinkyChild = _modelFingers[nPinkyPos].GetComponentsInChildren<Transform>();	
						
				_localEuler1[nThumbPos] = thmbChild[1].localEulerAngles;							
				_localEuler2[nThumbPos] = thmbChild[2].localEulerAngles;			
				
				_localEuler1[nIndexPos] = idxChild[1].localEulerAngles;							
				_localEuler2[nIndexPos] = idxChild[2].localEulerAngles;

				_localEuler1[nMiddlePos] = middChild[1].localEulerAngles;							
				_localEuler2[nMiddlePos] = middChild[2].localEulerAngles;
				
				_localEuler1[nRingPos] = ringChild[1].localEulerAngles;							
				_localEuler2[nRingPos] = ringChild[2].localEulerAngles;
				
				_localEuler1[nPinkyPos] = pinkyChild[1].localEulerAngles;							
				_localEuler2[nPinkyPos] = pinkyChild[2].localEulerAngles;				
			}
			
						
		}
		
		public void Pitch(float UpRotation, float DownRotation){
			UnityMax = UpRotation; 
			UnityMin = DownRotation;
			
			pitchA = (UnityMax-UnityMin)/(0.5f - (-0.5f));
			pitchB = UnityMin - pitchA*(-0.5f);
			
			//TraceDebug("pitchA = "+pitchA+" pitchB = " + pitchB); 
		}		
		public void Yaw(float RightRotation, float LeftRotation){
			UnityMax = LeftRotation; 
			UnityMin = RightRotation;
			
			yawA = (UnityMax-UnityMin)/(0.5f - (-0.5f));
			yawB = UnityMin - yawA*(-0.5f);
			
			//TraceDebug("yawA = "+yawA+" yawB = " + yawB); 
		}		
		public void Roll(float RightRotation, float LeftRotation){
			UnityMax = LeftRotation; 
			UnityMin = RightRotation;
			
			rollA = (UnityMax-UnityMin)/(1f - (-1f));
			rollB = UnityMin - rollA*(-1f);
			
			//TraceDebug("rollA = "+rollA+" rollB = " + rollB); 
		}
		
		//Allow null values or empty 
		public void Thumb(float Min, float Max, float Min1, float Max1, float Min2, float Max2){
			UnityMinFinger[nThumbPos] = Min;
			UnityMaxFinger[nThumbPos] = Max;
			
			UnityMinFinger1[nThumbPos] = Min1;
			UnityMaxFinger1[nThumbPos] = Max1;
			
			UnityMinFinger2[nThumbPos] = Min2;			
			UnityMaxFinger2[nThumbPos] = Max2;			
		}		
		public void Index(float Min, float Max, float Min1, float Max1, float Min2, float Max2){
			UnityMinFinger[nIndexPos] = Min;
			UnityMaxFinger[nIndexPos] = Max;
			
			UnityMinFinger1[nIndexPos] = Min1;
			UnityMaxFinger1[nIndexPos] = Max1;
			
			UnityMinFinger2[nIndexPos] = Min2;
			UnityMaxFinger2[nIndexPos] = Max2;		
		}		
		public void Middle(float Min, float Max, float Min1, float Max1, float Min2, float Max2){
			UnityMinFinger[nMiddlePos] = Min;
			UnityMaxFinger[nMiddlePos] = Max;
			
			UnityMinFinger1[nMiddlePos] = Min1;
			UnityMaxFinger1[nMiddlePos] = Max1;
			
			UnityMinFinger2[nMiddlePos] = Min2;
			UnityMaxFinger2[nMiddlePos] = Max2;		
		}
		public void Ring(float Min, float Max, float Min1, float Max1, float Min2, float Max2){
			UnityMinFinger[nRingPos] = Min;
			UnityMaxFinger[nRingPos] = Max;
			
			UnityMinFinger1[nRingPos] = Min1;			
			UnityMaxFinger1[nRingPos] = Max1;
			
			UnityMinFinger2[nRingPos] = Min2;
			UnityMaxFinger2[nRingPos] = Max2;			
		}
		public void Pinky(float Min, float Max, float Min1, float Max1, float Min2, float Max2){
			UnityMinFinger[nPinkyPos] = Min;
			UnityMaxFinger[nPinkyPos] = Max;
			
			UnityMinFinger1[nPinkyPos] = Min1;
			UnityMaxFinger1[nPinkyPos] = Max1;
			
			UnityMinFinger2[nPinkyPos] = Min2;
			UnityMaxFinger2[nPinkyPos] = Max2;			
		}
		//Wirst movement 
		//CHECK IF CAN BE DONE THE SAME FOR ARMS AND HANDS
		private void SetWirstNewAngle(){
			var args = _tareadQuartEvent as BoardQuaternionEventArgs; 
			float pitchAngle;
			float yawAngle;
			float rollAngle;
			
			if(args != null)
			{	
				//TraceDebug("- Stream Received : " + _tareadQuartEvent.StreamType.ToString());	
		
				float quaternionX = args.Value.X;
				float quaternionY = args.Value.Y;
				float quaternionZ = args.Value.Z;		
							
				pitchAngle 	= quaternionX*pitchA + pitchB;
				yawAngle	= quaternionY*yawA + yawB;	
				rollAngle 	= quaternionZ*rollA + rollB;	
			
				/*
				//Pitch when hand is upside down TODO IMPROVE THIS MOVEMENT 
				if((_enHandType == ModuleType.TYPE_LEFT_HAND &&	quaternionZ>0.9) ||
					(_enHandType == ModuleType.TYPE_RIGHT_HAND && quaternionZ<-0.9)) //Boca arriba
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
		
		private void AsignAngle2Axes(float pitchA, float yawA, float rollA){
			switch(_pitchAxis)
			{
				case ModuleAxis.AXIS_X:
					_x = pitchA;
					break;
				case ModuleAxis.AXIS_Y:
					_y = pitchA;
					break;
				case ModuleAxis.AXIS_Z:
					_z = pitchA; 
					break;
			}
			
			switch(_yawAxis)
			{
				case ModuleAxis.AXIS_X:
					_x = yawA;
					break;
				case ModuleAxis.AXIS_Y:
					_y = yawA;
					break;
				case ModuleAxis.AXIS_Z:
					_z = yawA; 
					break;
			}
			
			switch(_rollAxis)
			{
				case ModuleAxis.AXIS_X:
					_x = rollA;
					break;
				case ModuleAxis.AXIS_Y:
					_y = rollA;
					break;
				case ModuleAxis.AXIS_Z:
					_z = rollA; 
					break;
			}
		}
		
		public void Wirst(){			
			if(GetIsInitialized())
			{
				SetWirstNewAngle();			
			}
			
			_model.localEulerAngles = new UnityEngine.Vector3(_x, _y, _z);					 			
		}

		public void SetFingerSensors(int thumbP, int idxP, int middP, int rngP, int pinkP){
			nThumbPos 	= thumbP-1;
			nIndexPos 	= idxP-1;
			nMiddlePos 	= middP-1;
			nRingPos 	= rngP-1;
			nPinkyPos 	= pinkP-1; 
		}			
		
		private void GetAB(){			
			
			TraceDebug("Get AB ");
			float num = 0f; 
			
			for (int i = 0; i<10; i++){
				num = SensorMin[i] - SensorMax[i];
			
				if(num!=0f){
					a[i] = (UnityMaxFinger[i] - UnityMinFinger[i])/num;									
					a1[i] = (UnityMaxFinger1[i] - UnityMinFinger1[i])/num;									
					a2[i] = (UnityMaxFinger2[i] - UnityMinFinger2[i])/num;									
				}		
				
				b[i] = UnityMinFinger[i] - (a[i] * SensorMax[i]);
				b1[i] = UnityMinFinger1[i] - (a1[i] * SensorMax[i]);
				b2[i] = UnityMinFinger2[i] - (a2[i] * SensorMax[i]);
			}	

			if (sensorRead == true)
				abSet = true; 
		}
		
		//Fingers movement	
		private void SetFingersNewAngle(){
			var args = _sensorStateEvent as BoardFloatSequenceEventArgs;			
			float temp; 
			
			if(args != null){
								
				//CALCULATE JUST ONCE 
				if(abSet == false) {
					GetAB();
					for(int i = 0; i<10; i++)
					{
						triggers[i] = (SensorMax[i]-SensorMin[i])/3;
					}
				}
				
				for(int i = 0; i<10; i++)
				{	
					sensorVal[i] = args.Value[i];			
					temp = sensorVal[i]*a[i] + b[i];										
					
					//Avoid shaking
					if( Mathf.Abs(temp - _localEuler[i].x) > shakeDegree){
						_localEuler[i].x = temp; 
						_localEuler1[i].x = args.Value[i]*a1[i] + b1[i];
						_localEuler2[i].x = args.Value[i]*a2[i] + b2[i];					
					}					
					
					if(Mathf.Abs(_localEuler[i].x) < Mathf.Abs(UnityMinFinger[i])){
						_localEuler[i].x = UnityMinFinger[i];
						_localEuler1[i].x = UnityMinFinger1[i];
						_localEuler2[i].x = UnityMinFinger2[i];
					}
				}
			}
		}	
		
		//DO FOR EACH FINGER 
		private void SetPinkyNewAngle(){
			var args = _sensorStateEvent as BoardFloatSequenceEventArgs;			
			
			if(args != null){
				
				float pinkyVal = args.Value[nPinkyPos];	
				
				//CALCULATE JUST ONCE 
				GetAB();
				
				_localEuler[nPinkyPos].x 	= -(pinkyVal*a[nPinkyPos] + b[nPinkyPos]);
				
				//Debug.Log("pinkyVal: " + _xP);					
				
			}
		}			
		public UnityEngine.Vector3 Pinky(){
			
			if(GetIsInitialized())
			{
				SetPinkyNewAngle();			
			}
			
			return _localEuler[nPinkyPos];
		}
		
		public void Fingers(){
			
			if(GetIsInitialized()){
				
				SetFingersNewAngle();

				for(int i = 0; i<10; i++){
					if(_modelFingers[i] != null){
						_modelFingers[i].localEulerAngles 	= _localEuler[i];						
					}					
				}		
				
				thmbChild[1].localEulerAngles = _localEuler1[nThumbPos];
				thmbChild[2].localEulerAngles = _localEuler2[nThumbPos];
						
				idxChild[1].localEulerAngles = _localEuler1[nIndexPos];
				idxChild[2].localEulerAngles = _localEuler2[nIndexPos];
		 	
				middChild[1].localEulerAngles = _localEuler1[nMiddlePos];
				middChild[2].localEulerAngles = _localEuler2[nMiddlePos];
					
				ringChild[1].localEulerAngles = _localEuler1[nRingPos];
				ringChild[2].localEulerAngles = _localEuler2[nRingPos];
		
				pinkyChild[1].localEulerAngles = _localEuler1[nPinkyPos];
				pinkyChild[2].localEulerAngles = _localEuler2[nPinkyPos];
		
			} //Initialized
				
		}
		
		public bool isPressed(){
			bool bRet = false;
			
			if(GetIsInitialized()){			
				if(sensorVal[nPreassurePos]>triggers[nPreassurePos])
					bRet = true;
			}
			
			return bRet;			
		}
		
		public bool isCatch(){
			bool bRet = false;
			
			if(GetIsInitialized()){		
				if(	sensorVal[nIndexPos]	<triggers[nIndexPos] 	&&
					sensorVal[nMiddlePos]	<triggers[nMiddlePos] 	&&
					sensorVal[nRingPos]		<triggers[nRingPos] 	&&
					sensorVal[nPinkyPos]	<triggers[nPinkyPos]){
					
						bRet = true;
				}
			}			
			
			return bRet;
		}
		
		public bool isNumber1(){
			bool bRet = false;
			
			if(GetIsInitialized()){		
				if(	sensorVal[nIndexPos]	>triggers[nIndexPos] 	&&
					sensorVal[nMiddlePos]	<triggers[nMiddlePos] 	&&
					sensorVal[nRingPos]		<triggers[nRingPos] 	&&
					sensorVal[nPinkyPos]	<triggers[nPinkyPos]){
					
						bRet = true;
				}
			}			
			
			return bRet;
		}

	public bool isNumber2()
	{
		bool bRet = false;

		if (GetIsInitialized())
		{
			if (sensorVal[nIndexPos]	> triggers[nIndexPos] &&
				sensorVal[nMiddlePos]	> triggers[nMiddlePos] &&
				sensorVal[nRingPos]		< triggers[nRingPos] &&
				sensorVal[nPinkyPos]	< triggers[nPinkyPos])
			{
				bRet = true;
			}
		}
	
		return bRet;

	}
	public bool isNumber3()
	{
		bool bRet = false;

		if (GetIsInitialized())
		{
			if (sensorVal[nIndexPos]	> triggers[nIndexPos] &&
				sensorVal[nMiddlePos]	> triggers[nMiddlePos] &&
				sensorVal[nRingPos]		> triggers[nRingPos] &&
				sensorVal[nPinkyPos]	< triggers[nPinkyPos])
				{
					bRet = true;
				}
		}

		return bRet;

	}
	//Utility
	private static string FloatsToString(float[] value) {
			string result = "";
			var index = 0;
			foreach (var element in value) {
				if (index != 0) {
                result += ", ";
				}
				result += element.ToString();
				index += 1;
			}
			return result;
		}

    } //Class MyHand
//}	//namespace Hand
