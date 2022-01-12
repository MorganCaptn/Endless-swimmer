using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private bool player_collided;
    private int player_score = 0;
    public Material defeat_material;
    private MeshRenderer my_renderer;
    private Ring ring_script;
    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (!player_collided)
            {
                player_collided = true;
                my_renderer.material = defeat_material;
                Debug.Log("Player collided with obstacle!");
            }
        }

        if (other.gameObject.tag == "Ring")
        {
            ring_script = other.gameObject.GetComponent<Ring>();
            player_score += ring_script.GetPoints();
        }

    }

    public bool GetCollisionStatus()
    {
        return player_collided;
    }

    public int GetPlayerScore()
    {
        return player_score;
    }

    public void ResetObstacleCollision()
    {
        if (player_collided)
        {
            player_collided = false;
        }

    }
}
