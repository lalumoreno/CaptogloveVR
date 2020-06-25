using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform cam;
    public Transform attached1;
    public Transform attached2;

    private Vector3 camPos, attachedPos1, attachedPos2;
    private float step;
    // Start is called before the first frame update
    void Start()
    {
        step = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        camPos = cam.localPosition;
        attachedPos1 = attached1.localPosition;
        attachedPos2 = attached2.localPosition;

        if (Input.GetKey(KeyCode.W))
        {                
            camPos.z = camPos.z + step;
            attachedPos1.z = attachedPos1.z + step;
            attachedPos2.z = attachedPos2.z + step;
        }
        if (Input.GetKey(KeyCode.S))
        {
            camPos.z = camPos.z - step;
            attachedPos1.z = attachedPos1.z - step;
            attachedPos2.z = attachedPos2.z - step;
        }
        if (Input.GetKey(KeyCode.D))
        {
            camPos.x = camPos.x + step;
            attachedPos1.x = attachedPos1.x + step;
            attachedPos2.x = attachedPos2.x + step;
        }
        if (Input.GetKey(KeyCode.A))
        {
            camPos.x = camPos.x - step;
            attachedPos1.x = attachedPos1.x - step;
            attachedPos2.x = attachedPos2.x - step;
        }
        if (Input.GetKey(KeyCode.E))
        {
            camPos.y = camPos.y + step;
            attachedPos1.y = attachedPos1.y + step;
            attachedPos2.y = attachedPos2.y + step;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            camPos.y = camPos.y - step;
            attachedPos1.y = attachedPos1.y - step;
            attachedPos2.y = attachedPos2.y - step;
        }
        cam.localPosition = camPos;
        attached1.localPosition = attachedPos1;
        attached2.localPosition = attachedPos2;
    }
}
