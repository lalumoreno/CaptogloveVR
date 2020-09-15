using UnityEngine;

public class VRCamera : MonoBehaviour
{
    public Transform tCamera;
    public Transform tObject1;
    public Transform tObject2;

    private Vector3 vCameraPos, vObject1Pos, vObject2Pos;
    private float fStep;

    // Start is called before the first frame update
    void Start()
    {
        fStep = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        vCameraPos = tCamera.localPosition;
        vObject1Pos = tObject1.localPosition;
        vObject2Pos = tObject2.localPosition;

        if (Input.GetKey(KeyCode.W))
        {                
            vCameraPos.z = vCameraPos.z + fStep;
            vObject1Pos.z = vObject1Pos.z + fStep;
            vObject2Pos.z = vObject2Pos.z + fStep;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vCameraPos.z = vCameraPos.z - fStep;
            vObject1Pos.z = vObject1Pos.z - fStep;
            vObject2Pos.z = vObject2Pos.z - fStep;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vCameraPos.x = vCameraPos.x + fStep;
            vObject1Pos.x = vObject1Pos.x + fStep;
            vObject2Pos.x = vObject2Pos.x + fStep;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vCameraPos.x = vCameraPos.x - fStep;
            vObject1Pos.x = vObject1Pos.x - fStep;
            vObject2Pos.x = vObject2Pos.x - fStep;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            vCameraPos.y = vCameraPos.y + fStep;
            vObject1Pos.y = vObject1Pos.y + fStep;
            vObject2Pos.y = vObject2Pos.y + fStep;
        }
        if (Input.GetKey(KeyCode.E))
        {
            vCameraPos.y = vCameraPos.y - fStep;
            vObject1Pos.y = vObject1Pos.y - fStep;
            vObject2Pos.y = vObject2Pos.y - fStep;
        }
        tCamera.localPosition = vCameraPos;
        tObject1.localPosition = vObject1Pos;
        tObject2.localPosition = vObject2Pos;
    }
}
