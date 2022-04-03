using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int obstacle_passed;
    public int obstacles_for_next_level = 5;
    public int score;
    
    public GameObject player_object;
    public GameObject player_model;
    public float obstacle_movement_speed = 8.0f;
    public float ring_movement_speed = 8.0f;
    public float landscape_movement_speed = 8.0f;

    public float speed_increase_factor = 0.08f;
    public float spawn_distance_increase_factor = 0.25f;
    public float minimum_speed = 8f;
    public float maximum_speed = 12f;
    public float maximum_spawn_distance = 25f;
    private float spawn_distance = 15f;
    


    public int level;
    private int current_level = 0;

    [Header("FOR TESTING")]
    public bool player_invincible = false;
    public int start_level = 0;

    private ObstacleManager obs_manager;
    private RingManager ring_manager;
    private LandscapeManager landscape_manager;
    private GUIManager gui_manager;

    private Player_Control player_control;
    private PlayerModel player;

    // Start is called before the first frame update
    void Start()
    {

        obs_manager = gameObject.GetComponent<ObstacleManager>();
        ring_manager = gameObject.GetComponent<RingManager>();
        gui_manager = gameObject.GetComponent<GUIManager>();
        landscape_manager = gameObject.GetComponent<LandscapeManager>();

        player_control = player_object.GetComponent<Player_Control>();
        player = player_model.GetComponent<PlayerModel>();

        gui_manager.ActivateGUI(true);
        gui_manager.ActivateGameOverScreen(false);

        level = start_level;
        obs_manager.SetMovementSpeed(obstacle_movement_speed);
        ring_manager.SetMovementSpeed(ring_movement_speed);
        landscape_manager.SetMovementSpeed(landscape_movement_speed);
        spawn_distance = obs_manager.GetDistanceNextObstacleLine();
    }

    // Update is called once per frame
    void Update()
    {

        score = player.GetPlayerScore();
        obstacle_passed = player.GetObstacleCount();
        if (start_level == 0)
        {
            level = player.GetPlayerLevel();
            
        }


        if (current_level != level)
        {
            ChangeDifficulty(level);
            
        }
     
        if (obstacle_passed  == obstacles_for_next_level)
        {
            Debug.Log("Engaging LevelUp.");
            obs_manager.EngageNextLevel();
            obstacles_for_next_level += 5;
        }

        if (player.GetCollisionStatus() && !player_invincible)
        {
            
          
            StopGame();
            

        }
        else
        {
            gui_manager.PrintScore(score);
            gui_manager.PrintLevel(current_level);
        }

        

    }

    void ChangeDifficulty(int game_level)
    {

        //SPEED

        float new_speed;

        if ((game_level * speed_increase_factor) <= (maximum_speed - minimum_speed))
        {
            new_speed = obstacle_movement_speed + (game_level * speed_increase_factor);
            Debug.Log("Set new speed: " + new_speed.ToString());
            obs_manager.SetMovementSpeed(new_speed);
            ring_manager.SetMovementSpeed(new_speed);
            landscape_manager.SetMovementSpeed(new_speed);
        }

        //reached the speed limit
        else if ((game_level * speed_increase_factor) > (maximum_speed - minimum_speed))
        {
            Debug.Log("Set new speed: " + maximum_speed.ToString());
            obs_manager.SetMovementSpeed(maximum_speed);
            ring_manager.SetMovementSpeed(maximum_speed);
            landscape_manager.SetMovementSpeed(maximum_speed);
        }

        //OBSTACLES
        
        // If level gets higher, more difficult obstacles will be spawned and spawning distance will be reduced (0-20)
        if (game_level >= 0 && game_level <= 4)
        {
            Debug.Log("Set range to 0, 5");
            obs_manager.SetLevelRange(0, 5);
            
        }

        if (game_level >= 5 && game_level <= 10)
        {
            Debug.Log("Set range to 0, 10");
            obs_manager.SetLevelRange(0, 10);
        }

        if (game_level >= 11 && game_level <= 20)
        {
            Debug.Log("Set range to 0, 20");
            obs_manager.SetLevelRange(0, 20);
        }

        if (game_level >= 21 && game_level <= 25)
        {
            Debug.Log("Set range to 5, 25");
            obs_manager.SetLevelRange(5, 25);
        }

        if (game_level >= 50)
        {
            Debug.Log("Set range to 25, 50");
            obs_manager.SetLevelRange(25, 50);
        }

        //SPAWNING
        float new_distance;
        new_distance = obs_manager.GetDistanceNextObstacleLine() + spawn_distance_increase_factor;
    
        if (new_distance <= maximum_spawn_distance)
        {
            obs_manager.ChangeDistanceNextObstacleLine(spawn_distance_increase_factor);
        }
        

        current_level = game_level;    
        
    }

    void StopGame()
    {
        

        obs_manager.SetMovement(false);
        player_control.SetMovement(false);
        landscape_manager.SetMovement(false);
        ring_manager.SetMovement(false);
        gui_manager.PrintScore(score);
        gui_manager.PrintLevel(current_level);
        gui_manager.ActivateGUI(false);
        gui_manager.ActivateGameOverScreen(true);
        
    }

    void IncreaseScore()
    {
        score += 10;
    }

}
