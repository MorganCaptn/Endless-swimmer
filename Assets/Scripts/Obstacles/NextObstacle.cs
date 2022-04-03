using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextObstacle : MonoBehaviour
{
    private bool obstacle_collided = false;

    //Used for stopping (obstacle) spawning, f.e. during level change
    private bool special_event = false;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            if (!obstacle_collided)
            {
                obstacle_collided = true;
                Debug.Log("Obstacle passed the next spawn line!");
            }
        }

        if (other.gameObject.tag == "LevelUp")
        {
            Debug.Log("Level Up passed the next spawn line!");
            special_event = false;
        }
    }
    public bool GetObstacleCollisionStatus()
    {
        return obstacle_collided;
    }

    public bool GetSpecialEventStatus()
    {
        return special_event;
    }
    public void SetSpecialEventStatus(bool event_status)
    {
        special_event = event_status;
    }
    public void ResetObstacleCollision()
    {
        if (obstacle_collided)
        {
            obstacle_collided = false;
        }
       
    }

}
