using UnityEngine;

public class VRCamera : MonoBehaviour
{
    public Transform tCamera;
    public Transform tObject1;
    public Transform tObject2;

    private Vector3 vCameraPos, vObject1Pos, vObject2Pos;
    private Vector3 vCameraRot;
    private float fStep;

    // Start is called before the first frame update
    void Start()
    {
        fStep = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        vCameraPos = tCamera.localPosition;
        vObject1Pos = tObject1.localPosition;
        vObject2Pos = tObject2.localPosition;

        vCameraRot = tCamera.localEulerAngles;

        if (Input.GetKey(KeyCode.W))
        {
            if (vCameraPos.z < 18)
            {
                vCameraPos.z += fStep;
                vObject1Pos.z += fStep;
                vObject2Pos.z += fStep;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (vCameraPos.z > -13)
            {
                vCameraPos.z -= fStep;
                vObject1Pos.z -= fStep;
                vObject2Pos.z -= fStep;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (vCameraPos.x < 16)
            {
                vCameraPos.x += fStep;
                vObject1Pos.x += fStep;
                vObject2Pos.x += fStep;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (vCameraPos.x > -14.5)
            {
                vCameraPos.x -= fStep;
                vObject1Pos.x -= fStep;
                vObject2Pos.x -= fStep;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (vCameraPos.y < 22)
            {
                vCameraPos.y += fStep;
                vObject1Pos.y += fStep;
                vObject2Pos.y += fStep;
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            if (vCameraPos.y > 5.6)
            {
                vCameraPos.y -= fStep;
                vObject1Pos.y -= fStep;
                vObject2Pos.y -= fStep;
            }
        }
        else if(Input.GetKey(KeyCode.R))
        {
            //if(vCameraRot.x > -119f)
            {
                vCameraRot.x -= fStep;                
            }
        }
        else if (Input.GetKey(KeyCode.F))
        {
            //if (vCameraRot.x < -132f)
            {
                vCameraRot.x += fStep;                
            }
        }

        tCamera.localPosition = vCameraPos;
        tObject1.localPosition = vObject1Pos;
        tObject2.localPosition = vObject2Pos;
        tCamera.localEulerAngles = new Vector3(vCameraRot.x, vCameraRot.y, vCameraRot.z);
    }
}
