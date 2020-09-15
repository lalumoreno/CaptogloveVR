using UnityEngine;

public class Button : MonoBehaviour
{
    public Transform tButton;
    public Transform tDoor;

    public Vector3 vPressedPos, vReleasedPos; 
    private Vector3 vDoorUpPos, vDoorDownPos;

    // Start is called before the first frame update
    void Start()
    {
        vPressedPos = new Vector3(-11.88f, 9, 20.06f);
        vReleasedPos = new Vector3(-11.88f, 9, 19.48f);
        vDoorDownPos = new Vector3(-6.91f, 7.94f, 18.96f);
        vDoorUpPos = new Vector3(-6.91f, 12.68f, 18.96f);
    }

    // Update is called once per frame
    void Update()
    {
        if(tButton.localPosition == vPressedPos)        
            tDoor.localPosition = vDoorUpPos;        
        else        
            tDoor.localPosition = vDoorDownPos;         
    }
}
