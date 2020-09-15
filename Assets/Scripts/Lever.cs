using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform tDoor;
    public Transform tLever;

    private Vector3 vLeverPosition;
    private float fMaxYPos, fMinYPos, fXPos, fZPos;
        
    private Vector3 vDoorOpenPos;
    private Vector3 vDoorClosedPos;

    // Start is called before the first frame update
    void Start()
    {
        fMaxYPos = 0;
        fMinYPos = -2.84f;
        fXPos = 0;
        fZPos = 0;

        vDoorOpenPos = new Vector3(-1.48f, 10.68f, 19.91f);
        vDoorClosedPos = new Vector3(2.42f, 10.68f, 19.91f); 
    }

    // Update is called once per frame
    void Update()
    {
        LimitPosition();
        OpenDoor();
    }

    private void OpenDoor()
    {    
        if (tLever.localPosition.y >= fMaxYPos)
            tDoor.localPosition = vDoorClosedPos;
        else if (tLever.localPosition.y <= fMinYPos)
            tDoor.localPosition = vDoorOpenPos; 
    }

    private void LimitPosition()
    {
        vLeverPosition = tLever.localPosition;

        if (tLever.localPosition.y > fMaxYPos)        
            vLeverPosition.y = fMaxYPos;        
        else if (tLever.localPosition.y < fMinYPos)        
            vLeverPosition.y = fMinYPos;        

        tLever.localPosition = new Vector3(fXPos, vLeverPosition.y, fZPos) ;
    }
}
