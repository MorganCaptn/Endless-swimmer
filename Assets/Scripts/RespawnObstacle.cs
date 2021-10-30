using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObstacle : MonoBehaviour
{

    private GameObject collision_object;
    private bool obstacle_collided = false;
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
        Debug.Log("Wall collided with the cleanup line!!!!");
        if (!obstacle_collided)
        {
            obstacle_collided = true;
            collision_object = other.gameObject;
            Debug.Log("Wall collided with the cleanup line!");
        }
        

    }

    public bool GetCollisionStatus()
    {
        return obstacle_collided;
    }

    public GameObject GetColliderObject()
    {
        //TODO: Improve! Does not seem to be very efficient way of returning the wall movement script.

        if (collision_object.TryGetComponent<Wall_Movement>(out var script))
        {
            Debug.Log("GameObject has wallmovement script");
            return collision_object;
        }

        else if (collision_object.transform.parent.gameObject.TryGetComponent<Wall_Movement>(out script))
        {
            Debug.Log("GameObject PARENT has wallmovement script");
            return collision_object.transform.parent.gameObject;
        }
        Debug.Log("Retunred invalid respawn object.");
        return collision_object;

   
    }

    public void ResetObstacleCollision()
    {
        if (obstacle_collided)
        {
            obstacle_collided = false;
        }

    }
}
