using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRing : MonoBehaviour
{
    private GameObject collision_object;
    private bool ring_collided = false;
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
        if (other.gameObject.tag == "Ring")
        {
            Debug.Log("Ring collided with the respawn line!!!!");
            if (!ring_collided)
            {
                ring_collided = true;
                collision_object = other.gameObject;
                //TODO: Set respawn flag to true


               
            }
        }
    }

    public bool GetCollisionStatus()
    {
        return ring_collided;
    }

    public GameObject GetColliderObject()
    {
        //TODO: Improve! Does not seem to be very efficient way of returning the ring movement script.

        if (collision_object.TryGetComponent<Ring_Movement>(out var script))
        {
            Debug.Log("GameObject has ring movement script");
            return collision_object;
        }

        else if (collision_object.transform.parent.gameObject.TryGetComponent<Ring_Movement>(out script))
        {
            Debug.Log("GameObject PARENT has ring movement script");
            return collision_object.transform.parent.gameObject;
        }
        Debug.Log("Retunred invalid respawn object.");
        return collision_object;


    }

    public void ResetRingCollision()
    {
        if (ring_collided)
        {
            ring_collided = false;
        }

    }
}
