using UnityEngine;

public class myLever : MonoBehaviour
{
    public Transform tDoor;
    public bool bCollided;

    private Vector3 vLocalPosition;
    private float fMaxYPos, fMinYPos, fXPos, fZPos;        
    private Vector3 vDoorOpenPos;
    private Vector3 vDoorClosedPos;
    private Transform[] taChildren;

    // Start is called before the first frame update
    void Start()
    {
        fXPos = transform.localPosition.x;
        fZPos = transform.localPosition.z;
        fMaxYPos = transform.localPosition.y;
        fMinYPos = fMaxYPos - 2f;

        vDoorClosedPos = tDoor.localPosition;
        vDoorOpenPos = new Vector3(vDoorClosedPos.x-4, vDoorClosedPos.y, vDoorClosedPos.z);

        taChildren = transform.GetComponentsInChildren<Transform>();
        bCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        LimitPosition();
        OpenDoor();
    }

    private void OpenDoor()
    {    
        if (transform.localPosition.y >= fMaxYPos)
            tDoor.localPosition = vDoorClosedPos;
        else if (transform.localPosition.y <= fMinYPos)
            tDoor.localPosition = vDoorOpenPos; 
    }

    private void LimitPosition()
    {
        vLocalPosition = transform.localPosition;

        if (vLocalPosition.y > fMaxYPos)
            vLocalPosition.y = fMaxYPos;        
        else if (vLocalPosition.y < fMinYPos)
            vLocalPosition.y = fMinYPos;

        transform.localPosition = new Vector3(fXPos, vLocalPosition.y, fZPos) ;
    }

    public void ChangePosition(float vYPos)
    {
        transform.position = new Vector3(transform.position.x, vYPos, transform.position.z );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("myLever OnTriggerEnter!!");
            foreach (Transform go in taChildren)
            {
                go.GetComponent<Renderer>().material.color = Color.green;
            }

            bCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Transform go in taChildren)
            {
                go.GetComponent<Renderer>().material.color = Color.red;
            }
            bCollided = false;
        }
    }
}
