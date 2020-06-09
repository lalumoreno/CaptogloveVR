using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public Transform btn;
    public Transform door;

    private Vector3 pressedPos, releasedPos; 
    private Vector3 doorUpPos, doorDownPos;

    // Start is called before the first frame update
    void Start()
    {
        pressedPos = new Vector3(11.11f, 9, 20.22f);
        releasedPos = new Vector3(11.11f, 9, 19.48f);
        doorDownPos = new Vector3(-6.91f, 7.94f, 18.96f);
        doorUpPos = new Vector3(-6.91f, 12.68f, 18.96f);
    }

    // Update is called once per frame
    void Update()
    {
        if(btn.localPosition == pressedPos)
        {
            door.localPosition = doorUpPos;
        }
        else
        {
            door.localPosition = doorDownPos;
        }
    
    }
}
