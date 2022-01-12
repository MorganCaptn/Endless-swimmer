using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public GameObject player_object;
    public GameObject player_model;

    [Header("FOR TESTING")]
    public bool player_invincible = false;
    
    
    private ObstacleManager obs_manager;
    private GUIManager gui_manager;
    private Player_Control player_control;
    private PlayerModel player;

    // Start is called before the first frame update
    void Start()
    {
        obs_manager = gameObject.GetComponent<ObstacleManager>();
        gui_manager = gameObject.GetComponent<GUIManager>();
        player_control = player_object.GetComponent<Player_Control>();
        player = player_model.GetComponent<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {

        score = player.GetPlayerScore();

        
        if (player_control.GetCollisionStatus() && !player_invincible)
        {
            Debug.Log("Player defeated!");
            obs_manager.SetMovement(false);
            player_control.SetMovement(false);
            
        }
        else
        {
            gui_manager.PrintScore(score);
        }
        
        

    }

    void IncreaseScore()
    {
        score += 10;
    }

}
