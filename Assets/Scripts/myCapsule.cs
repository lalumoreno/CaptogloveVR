using System;
using UnityEngine;

public class myCapsule : MonoBehaviour
{
    public bool bCollided;
    private Renderer rCapsule;

    // Start is called before the first frame update
    void Start()
    {
        rCapsule = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ChangePosition(Vector3 vector3)
    {
        transform.position = vector3;
    }

    internal void ChangeRotation(Vector3 vector3)
    {
        transform.localEulerAngles = vector3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Capsule OnTriggerEnter!!");
            rCapsule.material.SetColor("_Color", Color.green);
            bCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            rCapsule.material.SetColor("_Color", Color.red);
            bCollided = false;
        }
    }
}
