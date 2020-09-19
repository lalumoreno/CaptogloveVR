using UnityEngine;

public class myLight : MonoBehaviour
{
    public bool bCollided;
    public Color cColor;
    private Renderer rLight;

    // Start is called before the first frame update
    void Start()
    {
        cColor = Color.red;
        rLight = transform.GetComponent<Renderer>();
        bCollided = false; 
    }

    // Update is called once per frame
    void Update()
    {
        rLight.material.SetColor("_Color", cColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Light OnTriggerEnter!!");
            bCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bCollided = false;
        }

    }

}
