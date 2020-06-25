using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public Transform capsule;
    private Renderer capsuleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        capsuleRenderer = capsule.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Capsule")
        {
            Debug.Log("OnTriggerEnter!!");
            capsuleRenderer.material.SetColor("_Color", Color.green);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Capsule")
        {
            capsuleRenderer.material.SetColor("_Color", Color.red);
        }
    }
}
