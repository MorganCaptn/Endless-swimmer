using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Respawn")
        {
            Debug.Log("Object collided with respawn line! Reset spawn flag.");
            Movement script = gameObject.transform.gameObject.GetComponent<Movement>();
            script.SetMovement(false);
            gameObject.SetActive(false);
        }

    }

}
