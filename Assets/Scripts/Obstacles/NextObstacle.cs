using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextObstacle : MonoBehaviour
{
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
        if (other.gameObject.tag == "Wall")
        {
            if (!obstacle_collided)
            {
                obstacle_collided = true;
                Debug.Log("Wall collided with next obstacle line!");
            }
        }
    }
    public bool GetCollisionStatus()
    {
        return obstacle_collided;
    }

    public void ResetObstacleCollision()
    {
        if (obstacle_collided)
        {
            obstacle_collided = false;
        }
       
    }
}
