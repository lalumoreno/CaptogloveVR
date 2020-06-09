using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform door;
    public Transform lever;

    private Vector3 LeverPosition;
    private float maxPos, minPos, xPos, zPos;

    //public float xRot;
    Quaternion test;
    private Vector3 openPosition;
    private Vector3 closePosition;

    // Start is called before the first frame update
    void Start()
    {
        maxPos = 1.66f;
        minPos = -1.66f;

        xPos = 0;
        zPos = 0.29f;

        openPosition = new Vector3(-1.48f, 10.68f, 19.91f);
        closePosition = new Vector3(2.42f, 10.68f, 19.91f); 
    }

    // Update is called once per frame
    void Update()
    {
        LimitPosition();
        OpenDoor();
    }

    private void OpenDoor()
    {
        LeverPosition = lever.localPosition;

        if (LeverPosition.y >= maxPos)
            door.localPosition = closePosition;
        else if (LeverPosition.y <= minPos)
            door.localPosition = openPosition; 
    }

    private void LimitPosition()
    {
        LeverPosition = lever.localPosition;

        if (LeverPosition.y > maxPos)
        {
            LeverPosition.y = maxPos;
        }
        if (LeverPosition.y < minPos)
        {
            LeverPosition.y = minPos;
        }

        lever.localPosition = new Vector3(xPos, LeverPosition.y, zPos) ;
    }
}
