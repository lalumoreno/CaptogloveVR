using UnityEditorInternal;
using UnityEngine;

public class RotTable : MonoBehaviour
{
    public Transform Stamp1;
    public Transform Stamp2;
    public Transform Stamp3;

    private float tiltAroundZ;
    private bool bRotate, bFirstTime, bIsUp;
    public float rotate;
    public float timer;
    public float waitingTime;

    private Vector3 maxPos, minPos; 
    
    // Start is called before the first frame update
    void Start()
    {
        tiltAroundZ = 0.5f;
        rotate = 0f;
        waitingTime = 5f;
        timer = 0f;
        maxPos = new Vector3(Stamp1.localPosition.x, Stamp1.localPosition.y, -0.06f);
        minPos = Stamp1.localPosition;

        bRotate = true;
        bFirstTime = true;
        bIsUp = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (bRotate)
        {
            rotate += tiltAroundZ;
            transform.Rotate(0, 0, tiltAroundZ);
            if (rotate == 90)
            {
                rotate = 0f;
                bRotate = false;
            }
        }
        else
        {
            if (bFirstTime)
            {
                if (bIsUp)
                    ArmDown();
                else
                    ArmUp();
            }

            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                timer = 0f;
                bRotate = true;
                bFirstTime = true;
            }
        }
    }

    void ArmDown()
    {
        Stamp1.Translate(Vector3.left * Time.deltaTime);
        Stamp2.Translate(Vector3.left * Time.deltaTime);
        Stamp3.Translate(Vector3.left * Time.deltaTime);
        Vector3 localPos = Stamp1.localPosition;

        if (localPos.z < maxPos.z)
            bIsUp = false;
    }
    void ArmUp()
    {        
        Stamp1.Translate(Vector3.right * Time.deltaTime);
        Stamp2.Translate(Vector3.right * Time.deltaTime);
        Stamp3.Translate(Vector3.right * Time.deltaTime);

        Vector3 localPos = Stamp1.localPosition;

        if (localPos.z > minPos.z)
        {
            bIsUp = true; 
            bFirstTime = false;
        }
    }
}
