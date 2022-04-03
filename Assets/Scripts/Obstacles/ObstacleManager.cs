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
    public bool force_spawn = true;
    public bool spawn_next_level = false;

    enum spawn_mode { none, obstacle, levelup};
    private spawn_mode current_spawn_mode = spawn_mode.none;
    private spawn_mode previous_spawn_mode = spawn_mode.none;
    //private states trigger_move = states.none;

    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(40, 40, 0);

    // Start is called before the first frame update
    void Start()
    {
        current_spawn_mode = spawn_mode.obstacle;
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

        if ((next_obstacle_script.GetObstacleCollisionStatus() || force_spawn == true))
        {
           
            if (current_spawn_mode == spawn_mode.obstacle && !next_obstacle_script.GetSpecialEventStatus() || force_spawn == true)
            {
                SpawnObstacle();
            }
            else if (current_spawn_mode == spawn_mode.levelup)
            {
                SpawnLevelUp();

            }
            
        }      
    }

    public void EngageNextLevel()
    {
        if (current_spawn_mode != spawn_mode.levelup)
        {
            current_spawn_mode = spawn_mode.levelup;
        }
      
    }
    public void SetLevelRange(int min, int max)
    {
        min_level = min;
        max_level = max;
    }

    public void ChangeDistanceNextObstacleLine(float z_diff)
    {
        next_obstacle_line.transform.position = new Vector3(next_obstacle_line.transform.position.x, next_obstacle_line.transform.position.y, (next_obstacle_line.transform.position.z + z_diff));
    }

    public float GetDistanceNextObstacleLine()
    {
        return next_obstacle_line.transform.localPosition.z;
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
            next_obstacle_script.SetSpecialEventStatus(true);
            
            //back to previous spawning
            current_spawn_mode = previous_spawn_mode;
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
            previous_spawn_mode = current_spawn_mode;
            force_spawn = false;
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
