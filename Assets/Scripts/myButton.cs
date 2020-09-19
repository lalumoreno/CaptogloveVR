using UnityEngine;

public class myButton : MonoBehaviour
{
    public Transform tDoor;
    public bool bCollided;

    private Vector3 vPressedPos, vReleasedPos;
    private Vector3 vDoorUpPos, vDoorDownPos;
    private bool bIsPressed;
    private Renderer rButton;

    // Start is called before the first frame update
    void Start()
    {
        vReleasedPos = transform.localPosition;
        vPressedPos = new Vector3(vReleasedPos.x, vReleasedPos.y, vReleasedPos.z + 0.5f);
        vDoorDownPos = tDoor.localPosition;
        vDoorUpPos = new Vector3(vDoorDownPos.x, vDoorDownPos.y + 4, vDoorDownPos.z);

        rButton = transform.GetComponent<Renderer>();
        bCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsPressed)
            tDoor.localPosition = vDoorUpPos;
        else
            tDoor.localPosition = vDoorDownPos;
    }

    public void PressButton()
    {
        transform.localPosition = vPressedPos;
        bIsPressed = true;
    }
    public void ReleaseButton()
    {
        transform.localPosition = vReleasedPos;
        bIsPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("myButton OnTriggerEnter!!");
            rButton.material.SetColor("_Color", Color.green);
            bCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            rButton.material.SetColor("_Color", Color.red);
            bCollided = false;
        }
    }
}
