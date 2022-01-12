using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //TODO: Do the same stuff like in the RingManager
    public GameObject[] obstacles;

    public GameObject spawn_line;
    public GameObject next_obstacle_line;
    public GameObject respawn_line;


    private GameObject[] instanciated_obstacles;
    private bool spawn_next_obstacle = true;
    private NextObstacle next_obst_script;
    private RespawnObstacle respawn_obst_script;

    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(40, 40, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateObstaclePool();
        next_obst_script = next_obstacle_line.GetComponent<NextObstacle>();
        respawn_obst_script = respawn_line.GetComponent<RespawnObstacle>();
        spawn_pos = spawn_line.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (respawn_obst_script.GetCollisionStatus())
        {
            //Get object ID of collider and set flag to respawn
            respawn_obst_script.GetColliderObject().GetComponent<Wall_Movement>().SetRespawnFlag(true);
            respawn_obst_script.ResetObstacleCollision();
        }

        if (next_obst_script.GetCollisionStatus())
        {
            spawn_next_obstacle = true;
            next_obst_script.ResetObstacleCollision();
        }



        if (spawn_next_obstacle)
        {
            spawn_next_obstacle = false;
            SpawnObstacle();
            
        }

    }

    void CreateObstaclePool()
    {
        //assuming each obstacle is available twice in the world
        instanciated_obstacles = new GameObject[obstacles.Length * 2];

        for (int i = 0; i < obstacles.Length; i++)
        {
            instanciated_obstacles[i * 2] = Instantiate(obstacles[i], pool_position, Quaternion.identity);
            instanciated_obstacles[(i * 2) + 1] = Instantiate(obstacles[i], pool_position, Quaternion.identity);
        }

    }

    public void ReturnObstacleToPool()
    {

    }

    public void SpawnObstacle()
    {

        //get a random number
        int random_pick = Random.Range(0, instanciated_obstacles.Length);

        //Access the obstacle script
        Wall_Movement script = instanciated_obstacles[random_pick].GetComponent<Wall_Movement>();


        //Check if pick is from pool
        if (script.GetPoolSpawnFlag())
        {
            Debug.Log("Pick is form pool.");
            instanciated_obstacles[random_pick].transform.position = spawn_pos;
            script.SetMovement(true);
            script.SetPoolSpawnFlag(false);
        }
        //Check if obstacle can already be respawned
        else if (script.GetRespawnFlag())
        {
            Debug.Log("Pick can be respawned");
            instanciated_obstacles[random_pick].transform.position = spawn_pos;
            script.SetMovement(true);
            script.SetRespawnFlag(false);
        }
        //Try another pick
        else
        {
            Debug.Log("Could not find a suitable pick.");
            spawn_next_obstacle = true;
        }


    }

    public void SetMovement(bool move)
    {
        for (int i = 0; i < instanciated_obstacles.Length; i++)
        {
            Wall_Movement script = instanciated_obstacles[i].GetComponent<Wall_Movement>();
            script.SetMovement(move);
        }
    }

   

   

}
