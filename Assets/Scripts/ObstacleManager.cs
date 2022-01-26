using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //TODO: Do the same stuff like in the RingManager
    public GameObject[] obstacles;

    public GameObject spawn_line;


    public float obstacle_cooldown = 5.0f;
    public float spawn_probability = 1.0f;

    private GameObject[] instanciated_obstacles;
    private bool spawn_cooldown = false;

    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(40, 40, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateObstaclePool();
        spawn_pos = spawn_line.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Set occurance probaility
        float probability = Random.Range(0.0f, 1.0f);
        float threshold = 1.0f - spawn_probability;

        if (probability >= threshold && !spawn_cooldown)
        {
            SpawnObstacle();
        }

        //Add respawn cooldown
        if (!spawn_cooldown)
        {
            StartCoroutine(CooldownTimer());
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

    private IEnumerator CooldownTimer()
    {
        spawn_cooldown = true;
        // then wait for it's cooldown.
        yield return new WaitForSeconds(obstacle_cooldown);
        spawn_cooldown = false;
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
    public void SetMovementSpeed(float speed)
    {
        for (int i = 0; i < instanciated_obstacles.Length; i++)
        {
            Wall_Movement script = instanciated_obstacles[i].GetComponent<Wall_Movement>();
            script.SetMovementSpeed(speed);
        }
    }





}
