using UnityEngine;
using System.ComponentModel; //needed for PropertyChangedEventArgs
using System.Linq; //Needed for String List 

using GSdkNet.Board;
using System;

namespace Arm
{	
	public class MyArm: MyModule
	{
		private ModuleType	_enArmType; 
			
		private Transform model; 
		private int BaseRotation; 
		
		private float UnityMax, UnityMin;		
		public float pitchA, pitchB, yawA, yawB; 
		
		//Constructor 
		public MyArm(int nID, bool rightArm){

			_enArmType = ModuleType.TYPE_LEFT_ARM; 
			
			if (rightArm){
				_enArmType = ModuleType.TYPE_RIGHT_ARM; 
			}
				
			InitModule(nID, _enArmType); 	 				
			
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

		//Wirst movement 
		//CHECK IF CAN BE DONE THE SAME FOR ARMS AND HANDS
		private void SetWirstNewAngle() {
			var args = _tareadQuartEvent as BoardQuaternionEventArgs;
			float pitchAngle;
			float yawAngle; 

			var args2 = _linearAccEvent as BoardFloatVectorEventArgs;

			if (args != null)
			{
				//TraceDebug("- Stream Received : " + _tareadQuartEvent.StreamType.ToString());	

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
						TraceDebug("Move arm backguard"); 
					}
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
			//		_x = rollA;
					break;
				case ModuleAxis.AXIS_Y:
			//		_y = rollA;
					break;
				case ModuleAxis.AXIS_Z:
			//		_z = rollA; 
					break;
			}
		}
		
		public void Wirst(){			
			if(GetIsInitialized())
			{
				SetWirstNewAngle();
				SetLinearPosition();
			}
			
			_model.localEulerAngles = new Vector3(_x, _y, _z);					 			
		}

        private void SetLinearPosition()
        {



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

}