using UnityEngine;
using GITEICaptoglove;

public class MainRightArm : MonoBehaviour
{
	private MyArm RightArm = null;
	private GUIStyle style;

	// Start is called before the first frame update
	void Start()
	{		
		RightArm = new MyArm(2502, MyArm.ArmType.TYPE_RIGHT_FOREARM);
		RightArm.SetLogEnabled(true);
		RightArm.SetArmObject(transform);

		//Messages in display
		style = new GUIStyle();
		style.normal.textColor = Color.black;

		RightArm.Start();
	}

	// Update is called once per frame
	void Update()
	{
		//Enables arm movement
		RightArm.MoveArm();
	}

	void OnGUI()
	{
		if (RightArm.GetPropertiesRead())
		{
			GUI.Label(new Rect(Screen.width - 100, 20, 200f, 200f), "Right Arm ready", style);
		}		
	}
	
	private void OnDestroy()
	{
		if(RightArm!= null)
			RightArm.Stop();
	}

}
