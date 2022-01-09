using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRing : MonoBehaviour
{
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

            if (!ring_collided)
            {
                ring_collided = true;
                Debug.Log("Ring collided with next ring line!");
            }
        }
    }

    public bool GetCollisionStatus()
    {
        return ring_collided;
    }

    public void ResetRingCollision()
    {
        if (ring_collided)
        {
            ring_collided = false;
        }

    }
}
