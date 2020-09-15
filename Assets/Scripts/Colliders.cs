using UnityEngine;

public class Colliders : MonoBehaviour
{
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

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.tag == "Player")                  
            rCapsule.material.SetColor("_Color", Color.red);        
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.tag == "Player")        
            rCapsule.material.SetColor("_Color", Color.white);        
    }
}
