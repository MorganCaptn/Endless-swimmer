using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public GameObject player_model;
    public float movspeed = 8f;
    public float rotspeed = 36f;
    public float max_move_left = -5.8f;
    public float max_move_right = 5.8f;
    public float reset_x_pos = 0f;
    public float reset_x_pos_threshold = 0.5f;
    public float magnet_strength = 1f;


    private bool collided_with_obstacle = false;
    private bool movement_allowed = true;
    private PlayerModel player_model_script;
    private Quaternion initial_rotation;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Created object!");

       initial_rotation = transform.rotation;
       player_model_script = player_model.gameObject.GetComponent<PlayerModel>();
        /*
        if (my_renderer != null)
        {
            Material current_material = my_renderer.material;
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(player_model_script.GetCollisionStatus())
        {
            Debug.Log("GOOOT IT");
            collided_with_obstacle = player_model_script.GetCollisionStatus();
        }
        */

        float h = Input.GetAxis("Horizontal") * movspeed;
        float r = Input.GetAxis("Horizontal") * rotspeed;
        //current position
        Vector3 playerPosition = transform.position;

        RotateBackToCenter(transform.rotation);
        
        if (movement_allowed)
        {
            if (playerPosition.x <= max_move_right && playerPosition.x >= max_move_left)
            {
                //add slight rotation to movement
                player_model.transform.Rotate(0, r * Time.deltaTime, 0);
                transform.Translate(h * Time.deltaTime, 0, 0);
            }
            else if (playerPosition.x > max_move_right)
            {
              
                

                transform.position = new Vector3(max_move_right, transform.position.y, transform.position.z);
            }
            else if (playerPosition.x < max_move_left)
            {
                transform.position = new Vector3(max_move_left, transform.position.y, transform.position.z);
            }
        }
        
    


    }



    /*
    private void MagnetBackToPos(Vector3 currentPos)
    {
        if (currentPos.x > reset_x_pos + reset_x_pos_threshold)
        {
            transform.Translate(-magnet_strength * Time.deltaTime, 0, 0);
        }
        if (currentPos.x < reset_x_pos - reset_x_pos_threshold)
        {
            transform.Translate(magnet_strength * Time.deltaTime, 0, 0);
        }
    }
    */
    public bool GetCollisionStatus()
    {
        return collided_with_obstacle;
    }

    private void RotateBackToCenter(Quaternion current_rotation)
    {
        Debug.Log("Rotate back");
 
    }
    /*
    public void SetCollisionStatus(bool is_collided)
    {
        collided_with_obstacle = is_collided;
    }
    */
    public void SetMovement(bool can_move)
    {
        movement_allowed = can_move;
    }
     
   


}
