using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring_BB : MonoBehaviour
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
            Debug.Log("Ring collided with respawn line!");
            //Assuming this script always has a parent with the ring movement script attached
            Movement script = gameObject.transform.GetComponent<Movement>();
            Ring child_script = gameObject.transform.GetChild(0).gameObject.transform.GetComponent<Ring>();
            script.SetMovement(false);
            child_script.Appear();
            gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Wall")
        {
            Movement script = gameObject.transform.GetComponent<Movement>();
            gameObject.SetActive(false);
            script.SetMovement(false);
        }
    }
}
