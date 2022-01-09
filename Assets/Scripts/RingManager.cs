using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public GameObject[] rings;

    public GameObject spawn_line;
    public GameObject next_ring_line;
    public GameObject respawn_line;


    private GameObject[] instanciated_rings;
    private bool spawn_next_ring = true;
    private NextRing next_ring_script;
    private RespawnRing respawn_ring_script;

    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(50, 50, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateRingPool();
        next_ring_script = next_ring_line.GetComponent<NextRing>();
        respawn_ring_script = respawn_line.GetComponent<RespawnRing>();
        spawn_pos = spawn_line.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (respawn_ring_script.GetCollisionStatus())
        {
            //Get object ID of collider and set flag to respawn
            respawn_ring_script.GetColliderObject().GetComponent<Ring_Movement>().SetRespawnFlag(true);
            respawn_ring_script.ResetRingCollision();
        }

        if (next_ring_script.GetCollisionStatus())
        {
            spawn_next_ring = true;
            next_ring_script.ResetRingCollision();
        }



        if (spawn_next_ring)
        {
            spawn_next_ring = false;
            SpawnRing();

        }

    }

    void CreateRingPool()
    {
        //assuming each obstacle is available twice in the world
        instanciated_rings = new GameObject[rings.Length * 2];

        for (int i = 0; i < rings.Length; i++)
        {
            instanciated_rings[i * 2] = Instantiate(rings[i], pool_position, Quaternion.identity);
            instanciated_rings[(i * 2) + 1] = Instantiate(rings[i], pool_position, Quaternion.identity);
        }

    }

    public void ReturnRingToPool()
    {

    }

    public void SpawnRing()
    {

        //get random numbers

        int random_pick = Random.Range(0, instanciated_rings.Length);
        float random_height = Random.Range(-3.8f, 3.8f);
        float random_width = Random.Range(-3.8f, 3.8f);
        Vector3 instance_pos = new Vector3(random_width, random_height, spawn_pos.z);
        //Access the obstacle script
        Ring_Movement script = instanciated_rings[random_pick].GetComponent<Ring_Movement>();


        //Check if pick is from pool
        if (script.GetPoolSpawnFlag())
        {
            Debug.Log("Pick is form pool.");
            instanciated_rings[random_pick].transform.position = instance_pos;
            script.SetMovement(true);
            script.SetPoolSpawnFlag(false);
        }
        //Check if obstacle can already be respawned
        else if (script.GetRespawnFlag())
        {
            Debug.Log("Pick can be respawned");
            instanciated_rings[random_pick].transform.position = instance_pos;
            script.SetMovement(true);
            script.SetRespawnFlag(false);
        }
        //Try another pick
        else
        {
            Debug.Log("Could not find a suitable pick.");
            spawn_next_ring = true;
        }


    }

    public void SetMovement(bool move)
    {
        for (int i = 0; i < instanciated_rings.Length; i++)
        {
            Ring_Movement script = instanciated_rings[i].GetComponent<Ring_Movement>();
            script.SetMovement(move);
        }
    }


}
