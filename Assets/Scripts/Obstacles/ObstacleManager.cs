using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacles;

    public GameObject spawn_line;
    public GameObject level_up_line;
    public GameObject next_obstacle_line;

    private int max_level = 5;
    private int min_level = 0;


    public float obstacle_cooldown = 5.0f;
    public float spawn_probability = 1.0f;

    private GameObject[] instanciated_obstacles;
    private GameObject instanciated_level_up_line;

    private NextObstacle next_obstacle_script;
    public bool initial_spawn = false;
    public bool spawn_next_level = false;
    public bool stop_spawning = false;

    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(40, 40, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateObstaclePool();
        spawn_pos = spawn_line.transform.position;
        next_obstacle_script = next_obstacle_line.GetComponent<NextObstacle>();
        SpawnObstacle();

        instanciated_level_up_line = Instantiate(level_up_line, pool_position, Quaternion.identity);
        instanciated_level_up_line.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Set occurance probaility
        float probability = Random.Range(0.0f, 1.0f);
        float threshold = 1.0f - spawn_probability;

        //CheckLevelUp();
        
        if((next_obstacle_script.GetObstacleCollisionStatus() || initial_spawn == false))
        {
            if (!spawn_next_level && next_obstacle_script.GetLevelUpCollisionStatus())
            {
                SpawnObstacle();
            }
            else
            {
               
                    SpawnLevelUp();
               
            }
            
        }      
    }

    private void CheckLevelUp()
    {
        if (spawn_next_level && next_obstacle_script.GetObstacleCollisionStatus())
        {
                Debug.Log("Spawning LevelUp Line");
                SpawnLevelUp();
        }

        if (instanciated_level_up_line.activeSelf)
        {
            //Wait for level up line to pass the next spawn line
            if (next_obstacle_script.GetLevelUpCollisionStatus())
            {
                stop_spawning = false;
            }
        }
    }
    public void EngageNextLevel()
    {
        if (!spawn_next_level)
        {
            spawn_next_level = true;
            stop_spawning = true;
        }
      
    }

    public void SetLevelRange(int min, int max)
    {
        min_level = min;
        max_level = max;
    }

    void CreateObstaclePool()
    {
        //assuming each obstacle is available twice in the world
        instanciated_obstacles = new GameObject[obstacles.Length * 2];

        for (int i = 0; i < obstacles.Length; i++)
        {
            instanciated_obstacles[i * 2] = Instantiate(obstacles[i], pool_position, Quaternion.identity);
            instanciated_obstacles[(i * 2) + 1] = Instantiate(obstacles[i], pool_position, Quaternion.identity);
            instanciated_obstacles[i * 2].SetActive(false);
            instanciated_obstacles[(i * 2) + 1].SetActive(false);
        }

    }
    private void SpawnLevelUp()
    {

        if (!instanciated_level_up_line.activeSelf)
        {
            Movement movement_script = instanciated_level_up_line.GetComponent<Movement>();

            instanciated_level_up_line.SetActive(true);
            instanciated_level_up_line.transform.position = spawn_pos;
            movement_script.SetMovement(true);
            next_obstacle_script.ResetLevelUpCollision();
            spawn_next_level = false;
        }
 
    }
    public void SpawnObstacle()
    {

        //get a random number
        int random_pick = Random.Range(0, instanciated_obstacles.Length);

        //Access the obstacle script
        Movement movement_script = instanciated_obstacles[random_pick].GetComponent<Movement>();
        Obstacle obstacle_script = instanciated_obstacles[random_pick].GetComponent<Obstacle>();
        int obstacle_level = obstacle_script.GetDifficultyLevel();

        Debug.Log(max_level);
        Debug.Log(min_level);

        if (!instanciated_obstacles[random_pick].activeSelf &&
            (obstacle_level <= max_level) &&
            (obstacle_level >= min_level))
        {
            instanciated_obstacles[random_pick].SetActive(true);
            instanciated_obstacles[random_pick].transform.position = spawn_pos;
            movement_script.SetMovement(true);
            next_obstacle_script.ResetObstacleCollision();
            initial_spawn = true;
        }
        else
        {
            Debug.Log("Could not find a suitable pick.");
        }


    }
    public void SetMovement(bool move)
    {
        for (int i = 0; i < instanciated_obstacles.Length; i++)
        {
            Movement movement_script = instanciated_obstacles[i].GetComponent<Movement>();
            movement_script.SetMovement(move);
        }

        Movement movement_script_lvl = instanciated_level_up_line.GetComponent<Movement>();
        movement_script_lvl.SetMovement(move);
    }
    public void SetMovementSpeed(float speed)
    {
        for (int i = 0; i < instanciated_obstacles.Length; i++)
        {
            Movement movement_script = instanciated_obstacles[i].GetComponent<Movement>();
            movement_script.SetMovementSpeed(speed);
        }

        Movement movement_script_lvl = instanciated_level_up_line.GetComponent<Movement>();
        movement_script_lvl.SetMovementSpeed(speed);
    }





}
