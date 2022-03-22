using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextObstacle : MonoBehaviour
{
    private bool obstacle_collided = false;
    private bool level_up_collided = false;
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
            level_up_collided = true;
        }

    }
    public bool GetObstacleCollisionStatus()
    {
        return obstacle_collided;
    }

    public bool GetLevelUpCollisionStatus()
    {
        return level_up_collided;
    }
    public void ResetObstacleCollision()
    {
        if (obstacle_collided)
        {
            obstacle_collided = false;
        }
       
    }

    public void ResetLevelUpCollision()
    {
        if (level_up_collided)
        {
            level_up_collided = false;
        }

    }
}
