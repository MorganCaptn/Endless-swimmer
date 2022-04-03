using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public float movespeed = 3.0f;
    public bool movement = false;
    public int move_direction=0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (movement && move_direction==0)
        {
            transform.Translate(Vector3.back * movespeed * Time.deltaTime);

        }
        else if (movement && move_direction == 1)
        {
            transform.Translate(Vector3.right * movespeed * Time.deltaTime);

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

    public void SetMovementSpeed(float speed)
    {
        movespeed = speed;
    }

  

}
