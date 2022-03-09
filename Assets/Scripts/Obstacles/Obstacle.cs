using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "Respawn")
        {
            //Change back to original appearance
            Debug.Log("Obstacle collided with respawn line! Reset spawn flag.");
            //Assuming this script always has a parent with the ring movement script attached
            Wall_Movement script = gameObject.transform.gameObject.GetComponent<Wall_Movement>();
            script.SetRespawnFlag(true);
            script.SetMovement(false);
        }
        
    }

}
