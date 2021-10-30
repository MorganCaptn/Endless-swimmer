using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Movement : MonoBehaviour
{
    public int movespeed = 3;
    private bool movement = false;
    private bool respawn = false;
    private bool spawned_from_pool = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movement)
        {
            transform.Translate(Vector3.back * movespeed * Time.deltaTime);

        }
    }

    public void SetMovement(bool move)
    {
        movement = move;
    }

    public bool GetMovement()
    {
        return movement;
    }

    //Are typically set by the ObstacleManager
    public void SetRespawnFlag(bool can_be_respawned)
    {
        respawn = can_be_respawned;
    }

    public void SetPoolSpawnFlag(bool spawned)
    {
        spawned_from_pool = spawned;
    }

    public bool GetPoolSpawnFlag()
    {
        return spawned_from_pool;
    }


    public bool GetRespawnFlag()
    {
        return respawn;
    }


}
