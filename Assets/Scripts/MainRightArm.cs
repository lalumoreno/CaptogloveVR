using UnityEngine;

public class MainRightArm : MonoBehaviour
{
	public Transform tArm;
	private MyArm RightArm = null;

	// Start is called before the first frame update
	void Start()
	{		
		RightArm = new MyArm(2502, MyArm.eArmType.TYPE_RIGHT_FOREARM);
		RightArm.EnableLog();
		RightArm.SetArmTransform(tArm, Module.eModuleAxis.AXIS_Y, Module.eModuleAxis.AXIS_Z, Module.eModuleAxis.AXIS_X);
		
		RightArm.Start();
	}

	// Update is called once per frame
	void Update()
	{
		//Enables arm movement
		RightArm.MoveArm();
	}

	private void OnDestroy()
	{
		if(RightArm!= null)
			RightArm.Stop();
	}

}
