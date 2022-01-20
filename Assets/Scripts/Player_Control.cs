using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public GameObject player_model;
    public float movspeed = 8f;
    public float rotspeed = 240f;
    public float rotbackspeed = 10f;

    public float max_move_left = -5.8f;
    public float max_move_right = 5.8f;
    public float reset_x_pos = 0f;
    public float reset_x_pos_threshold = 0.5f;

    public float gravity = -9.81f;
    public float gravityScale = 5f;

    public float jump_force = 20f;
    public float super_jump_force = 40f;
    public float jump_pitch_factor = -0.1f;
    public float super_jump_pitch_factor = -0.2f;
    private float velocity_jump;
    private float velocity_super_jump;
    private bool jump_active = false;
    private bool super_jump_active = false;
   

    public float dive_force = -20f;
    private float velocity_dive;
    private bool dive_active = false;

    private readonly float ground_height = 1.61f;
    public float height_threshold_for_super_jump = 1.5f;
    private bool is_grounded = false;

    private bool collided_with_obstacle = false;
    private bool movement_allowed = true;
    private PlayerModel player_model_script;
    private Quaternion initial_rotation;
 
    //for swiping control (mobile)
    private float startpos;
    private int pos;
    private float[] positionsset;
    private float h=0.0f;
    private float r=0.0f;

    private float touch_time_start, touch_time_finish, time_interval;
    private Vector2 start_pos, end_pos, direction;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Created object!");

       initial_rotation = player_model.transform.rotation;
       player_model_script = player_model.gameObject.GetComponent<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        is_grounded = false;

        //float h = Input.GetAxis("Horizontal") * movspeed;
        //float r = Input.GetAxis("Horizontal") * rotspeed;
        //Debug.Log(Input.GetAxis("Horizontal"));
        //SWIPE
        GetSwipeForce();

        if (Input.GetMouseButtonDown(0))
        {
            //normalize between -1 and 1
            float min = -1.0f;
            float max = 1.0f;
            float norm = Input.mousePosition.x / Screen.width;
            norm = norm * (max - min) + min;
            Debug.Log(norm);
            // Debug.Log(Screen.width);
            h = norm * movspeed;
            r = norm * rotspeed;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //normalize between -1 and 1
            float min = -1.0f;
            float max = 1.0f;
            float norm = Input.mousePosition.x / Screen.width;
            norm = norm * (max - min) + min;
            Debug.Log(norm);
            // Debug.Log(Screen.width);
            h = norm * movspeed;
            r = norm * rotspeed;
        }


        //current position
        Vector3 playerPosition = transform.position;

        RotateBackToCenter(player_model.transform.rotation);
   
        if (!jump_active && !super_jump_active)
        {
            Dive();
        }
        if (!dive_active && !super_jump_active)
        {
            Jump();
        }
        if (super_jump_active)
        {
            Debug.Log("Perform super jump!!");
            Debug.Log(transform.position.y);
            SuperJump();
        }
        


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
    private void GetSwipeForce()
    {
        //if you touch the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            start_pos = Input.GetTouch(0).position;
            float min = -1.0f;
            float max = 1.0f;
            float norm = start_pos.x / Screen.width;
            norm = norm * (max - min) + min;
            h = norm * movspeed;
            r = norm * rotspeed;
            //getting touch position and marking time when you touch the screen
            //touch_time_start = Time.time;
            //start_pos = Input.GetTouch(0).position;
        }

        //if you release your finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            h = 0;
            r = 0;
            //marking time when you release it
            //touch_time_finish = Time.time;

            //calculate swipe time interval
            //time_interval = touch_time_finish - touch_time_start;

            //getting release finger position
            //end_pos = Input.GetTouch(0).position;

            //calculating swipe direction in 2D space
            //direction = start_pos - end_pos;
            //float min = -1.0f;
            //float max = 1.0f;
            //float norm = direction.x / Screen.width;
            //norm = norm * (max - min) + min;
            //h = norm * movspeed;
            //r = norm * rotspeed;

        }
    }
    private void ResetToHeight(float height)
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void Dive()
    {

        velocity_dive -= gravity * gravityScale * Time.deltaTime;

        // Check for super jump
        if ((transform.position.y >= (ground_height - height_threshold_for_super_jump)) 
            && (transform.position.y < ground_height) 
            && Input.GetMouseButtonDown(1) && !is_grounded)
        {
            Debug.Log("SuperJump");
            super_jump_active = true;
            dive_active = false;
            velocity_super_jump = super_jump_force;
            ResetToHeight(ground_height);
        }


        if (transform.position.y >= ground_height)
        {
            is_grounded = true;
            ResetToHeight(ground_height);
            dive_active = false;
        }

       
        if (is_grounded && velocity_dive > 0)
        {
            velocity_dive = 0;
        }

        // Do not allow multiple jumps
        if (Input.GetKeyDown(KeyCode.DownArrow) && is_grounded)
        {
            velocity_dive = dive_force;
            dive_active = true;
        }

        transform.Translate(new Vector3(0, velocity_dive, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_dive, 0, 0) * jump_pitch_factor);

    }
    private void SuperJump()
    {

        velocity_super_jump += gravity * gravityScale * Time.deltaTime;

       

        if (is_grounded && velocity_super_jump < 0)
        {
            velocity_super_jump = 0;
        }

   
        transform.Translate(new Vector3(0, velocity_super_jump, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_super_jump, 0, 0) * super_jump_pitch_factor);

        if (transform.position.y <= ground_height)
        {
            Debug.Log("Resetted");
            is_grounded = true;
            ResetToHeight(ground_height);
            super_jump_active = false;
        }

    }


    private void Jump()
    {

        velocity_jump += gravity * gravityScale * Time.deltaTime;

        if (transform.position.y <= ground_height)
        {
            is_grounded = true;
            ResetToHeight(ground_height);
            jump_active = false;
        }

        if (is_grounded && velocity_jump < 0)
        {
            velocity_jump = 0;
        }
        
        // Do not allow multiple jumps
        if (Input.GetMouseButtonDown(1) && is_grounded)
        {
            velocity_jump = jump_force;
            jump_active = true;
        }

        transform.Translate(new Vector3(0, velocity_jump, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_jump, 0, 0) * jump_pitch_factor);

    }

    public bool GetCollisionStatus()
    {
        return collided_with_obstacle;
    }

    private void RotateBackToCenter(Quaternion current_rotation)
    {
        player_model.transform.rotation = Quaternion.Lerp(current_rotation, initial_rotation, rotbackspeed * Time.deltaTime);
    }

    public void SetMovement(bool can_move)
    {
        movement_allowed = can_move;
    }
     
   


}
