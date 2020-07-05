using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    private Renderer capsuleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        capsuleRenderer = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("OnTriggerEnter!!");
            capsuleRenderer.material.SetColor("_Color", Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            capsuleRenderer.material.SetColor("_Color", Color.white);
        }
    }
}
