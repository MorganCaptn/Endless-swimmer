using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {

            //Change back to original appearance
            Debug.Log("Landscape collided with respawn line!");
            //Assuming this script always has a parent with the ring movement script attached
            Movement script = gameObject.transform.GetComponent<Movement>();
            script.SetRespawnFlag(true);

        }
    }
}
