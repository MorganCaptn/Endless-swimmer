using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private bool player_collided;
    private int player_score = 0;
    private int player_level = 0;
    private int obstacle_passed = 0;
    public Material defeat_material;
    private MeshRenderer my_renderer;
    
    private LineRenderer line_renderer;
    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<MeshRenderer>();

        line_renderer = gameObject.AddComponent<LineRenderer>();

        line_renderer.startColor = Color.yellow;
        line_renderer.endColor = Color.yellow;
        line_renderer.startWidth = 0.04f;
        line_renderer.endWidth = 0.04f;
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> pos = new List<Vector3>();
        pos.Add(new Vector3(gameObject.transform.position.x, 1.61f, gameObject.transform.position.z));
        pos.Add(gameObject.transform.position);

        line_renderer.SetPositions(pos.ToArray());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (!player_collided)
            {
                my_renderer.material = defeat_material;
                Debug.Log("Player collided with obstacle!");
                SetCollisionStatus(true);
            }
        }

        if (other.gameObject.tag == "Ring")
        {
            Ring ring_script;
            ring_script = other.gameObject.GetComponent<Ring>();
            player_score += ring_script.GetPoints();
        }

        if (other.gameObject.tag == "SuperRing")
        {
            SuperRing super_ring_script;
            super_ring_script = other.gameObject.GetComponent<SuperRing>();
            player_score += super_ring_script.GetPoints();
        }

        if (other.gameObject.tag == "LevelUp")
        {
            Debug.Log("Player collided with level up line.");
            IncreaseLevel(1);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            obstacle_passed += 1;
        }

    }

    public int GetObstacleCount()
    {
        return obstacle_passed;
    }

    public int GetPlayerLevel()
    {
        return player_level;
    }

    private void IncreaseLevel(int level_up)
    {
        if (level_up > 0)
        {
            player_level += level_up;
            Debug.Log("Player Level: " + player_level.ToString());
        }
    }

    public void SetCollisionStatus(bool is_collided)
    {
        player_collided = is_collided;
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
