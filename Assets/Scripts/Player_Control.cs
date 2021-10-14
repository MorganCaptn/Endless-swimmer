using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public float movspeed = 8f;
    public float max_move_left = -5.8f;
    public float max_move_right = 5.8f;
    public float reset_x_pos = 0f;
    public float reset_x_pos_threshold = 0.5f;
    public float magnet_strength = 1f;
    public Material defeat_material;
    

    private MeshRenderer my_renderer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Created object!");
        my_renderer = GetComponent<MeshRenderer>();
        
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
        float h = Input.GetAxis("Horizontal") * movspeed;
       
        //current position
        Vector3 playerPosition = transform.position;
        MagnetBackToPos(playerPosition);

        Debug.Log(playerPosition.x);
        if (playerPosition.x <= max_move_right && playerPosition.x >= max_move_left)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object collided!");
        my_renderer.material = defeat_material;
        /*
        if (collision.collider.name == "BlueObject")
        {
            Debug.Log(collision.collider.name);
            Debug.Log("Impulse: " + collision.impulse);
            Debug.Log("Relative Velocity" + collision.relativeVelocity);
        }
        */
    }

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

}
