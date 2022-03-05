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
    public float super_jump_from_dive_threshold = -1.0f;
    private bool super_jump_possible = false;

    public float dive_force = -20f;
    private float velocity_dive;
    enum states {none, ground, jump, dive, super_jump};
    private states current_move_state = states.ground;
    private states trigger_move = states.none;

    private readonly float ground_height = 1.61f;
    public float height_threshold_for_super_jump = 1.5f;

    private bool collided_with_obstacle = false;
    private bool movement_allowed = true;
    private PlayerModel player_model_script;
    private Quaternion initial_rotation;

    private Rect button_area_jump;
    private Rect debug_button_area_jump;

    private Rect button_area_dive;
    private Rect debug_button_area_dive;

    
    //height and rotation of player
    private float h = 0.0f;
    private float r = 0.0f;

    private Vector2 start_pos;

    // Start is called before the first frame update
    void Start()
    {
        initial_rotation = player_model.transform.rotation;
        player_model_script = player_model.gameObject.GetComponent<PlayerModel>();
        
        float button_area_width = Screen.width/3;
        float button_area_height = Screen.height/10;

        // origin of x and y in bottom left corner
        button_area_dive = new Rect(Screen.width / 20, Screen.height/20, button_area_width, button_area_height);
        // GUI box origin of x and y in top left corner
        debug_button_area_dive = new Rect(Screen.width / 20, Screen.height - 2 * (Screen.height / 20), button_area_width, button_area_height);

        // origin of x and y in bottom left corner
        button_area_jump = new Rect(Screen.width - button_area_width - Screen.width / 20, Screen.height / 20, button_area_width, button_area_height);
        // GUI box origin of x and y in top left corner, so the position has to differ to make it match the button area
        debug_button_area_jump = new Rect(Screen.width - button_area_width - Screen.width / 20, Screen.height - 2 * (Screen.height / 20), button_area_width, button_area_height);


    }
    void OnGUI()
    {
        GUI.Box(debug_button_area_jump, "Jump");
        GUI.Box(debug_button_area_dive, "Dive");
    }
        // Update is called once per frame
    
    void Update()
    {

       
        GetHorizontalMovement();
      
        //current position
        Vector3 playerPosition = transform.position;

        RotateBackToCenter(player_model.transform.rotation);

        GetMovementState();
     
        if (current_move_state == states.jump)
        {
            Jump();
        }

        if (current_move_state == states.dive)
        {
            Dive();
        }

        if (current_move_state == states.super_jump)
        {
            Debug.Log("SuperJump");
            SuperJump();
        }

        if (movement_allowed)
        {
            if (playerPosition.x <= max_move_right && playerPosition.x >= max_move_left)
            {
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

    private void GetMovementState()
    {
        states touch_move = GetButtonInputTouch();
        
        //FROM GROUND
        if (current_move_state == states.ground)
        {

            // -> JUMP
            if (Input.GetKey(KeyCode.Space) || touch_move == states.jump)
            {               
                trigger_move = states.jump;
                current_move_state = states.jump;
            }

            // -> DIVE
            else if (Input.GetKey(KeyCode.DownArrow) || touch_move == states.dive)
            {
                trigger_move = states.dive;
                current_move_state = states.dive;
            }

        }

        //FROM DIVE
        if (current_move_state == states.dive)
        {
            // -> JUMP
            if ((Input.GetKey(KeyCode.Space) || touch_move == states.jump) && super_jump_possible)
            {
                trigger_move = states.super_jump;
                current_move_state = states.super_jump;
            }
        }
    }
  


    private states CheckButtons(Touch touch)
    {
        if (button_area_jump.Contains(touch.position))
        {

            if (touch.phase == TouchPhase.Began)
            {

                return states.jump;
            }

        }
        else if (button_area_dive.Contains(touch.position))
        {
      
            if (touch.phase == TouchPhase.Began)
            {

                return states.dive;
            }
        }
        //Debug.Log("Returned: None!");
        return states.none;
  
    
    }
    private states GetButtonInputTouch()
    {
        states state = states.none;
        if (Input.touchCount > 0)
        {
            //states state = states.none;
            
            var tapCount = Input.touchCount;
            //Debug.Log(tapCount);
            for (var i = 0; i < tapCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    state = CheckButtons(touch);
           
                }
                    
                if (state != states.none)
                    {
                    break;
                    }
            }
        }

        return state;

    }

    private void GetHorizontalMovement()
    {
        if (Input.touchCount > 0)
        {
            var tapCount = Input.touchCount;
            //Debug.Log(tapCount);
            for (var i = 0; i < tapCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (!(button_area_jump.Contains(touch.position)) && !(button_area_dive.Contains(touch.position)))
                {
                    //if you touch the screen
                    if (touch.phase == TouchPhase.Began)
                    {
                        start_pos = touch.position;
                        float min = -1.0f;
                        float max = 1.0f;
                        float norm = start_pos.x / Screen.width;
                        norm = norm * (max - min) + min;
                        h = norm * movspeed;
                        r = norm * rotspeed;

                    }

                    //TODO: Maybe different to single touch above
                    if (touch.phase == TouchPhase.Moved)
                    {
                        start_pos = touch.position;
                        float min = -1.0f;
                        float max = 1.0f;
                        float norm = start_pos.x / Screen.width;
                        norm = norm * (max - min) + min;
                        h = norm * movspeed;
                        r = norm * rotspeed;
                    }

                    //if you release your finger
                    if (touch.phase == TouchPhase.Ended)
                    {
                        h = 0;
                        r = 0;
                    }

                }

            }
        }
    }
    private void ResetToHeight(float height)
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void Jump()
    {

        if (trigger_move == states.jump)
        {
            velocity_jump = jump_force;
            trigger_move = states.none;
        }

        velocity_jump += gravity * gravityScale * Time.deltaTime;


        transform.Translate(new Vector3(0, velocity_jump, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_jump, 0, 0) * jump_pitch_factor);

        if (trigger_move == states.none && transform.position.y <= ground_height)
        {
            current_move_state = states.ground;
            ResetToHeight(ground_height);
        }

    }

    private void Dive()
    {
        if (trigger_move == states.dive)
        {
            velocity_dive = dive_force;
            trigger_move = states.none;
        }

        velocity_dive -= gravity * gravityScale * Time.deltaTime;

        transform.Translate(new Vector3(0, velocity_dive, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_dive, 0, 0) * jump_pitch_factor);

        if(velocity_dive >= super_jump_from_dive_threshold)
        {
            super_jump_possible = true;
        }
        else
        {
            super_jump_possible = false;
        }

        if (trigger_move == states.none && transform.position.y >= ground_height)
        {
            velocity_dive = 0.0f;
            current_move_state = states.ground;
            ResetToHeight(ground_height);
        }
        
 
    }
    private void SuperJump()
    {

        if (trigger_move == states.super_jump)
        {
            velocity_super_jump = super_jump_force;
            trigger_move = states.none;
        }

       

        velocity_super_jump += gravity * gravityScale * Time.deltaTime;

        transform.Translate(new Vector3(0, velocity_super_jump, 0) * Time.deltaTime);
        player_model.transform.Rotate(new Vector3(velocity_super_jump, 0, 0) * super_jump_pitch_factor);

        
        if (trigger_move == states.none && transform.position.y <= ground_height && velocity_super_jump <= 0)
        {
            velocity_super_jump = 0;
            current_move_state = states.ground;
            ResetToHeight(ground_height);
        }
        Debug.Log(velocity_super_jump);

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
