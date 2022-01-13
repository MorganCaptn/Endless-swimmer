using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public GameObject[] rings;

    public GameObject spawn_line;

    public float ring_cooldown = 5.0f;
    public float spawn_probability = 1.0f;

    private GameObject[] instanciated_rings;
    private bool spawn_cooldown = false;
 
    private Vector3 spawn_pos;
    private Vector3 pool_position = new Vector3(50, 50, 0);

    // Start is called before the first frame update
    void Start()
    {
        CreateRingPool();
        spawn_pos = spawn_line.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //Set occurance probaility
        float probability = Random.Range(0.0f, 1.0f);
        float threshold = 1.0f - spawn_probability;
  
        if (probability >= threshold && !spawn_cooldown)
        {
            SpawnRing();
        }

        //Add respawn cooldown
        if (!spawn_cooldown)
        {
            StartCoroutine(CooldownTimer());
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

    private IEnumerator CooldownTimer()
    {
        spawn_cooldown = true;
        // then wait for it's cooldown.
        yield return new WaitForSeconds(ring_cooldown);
        spawn_cooldown = false;
    }



    public void SpawnRing()
    {

        //get random numbers
        int random_pick = Random.Range(0, instanciated_rings.Length);
        float random_height = Random.Range(-1.8f, 3.8f);
        float random_width = Random.Range(-3.8f, 3.8f);
        Vector3 instance_pos = new Vector3(random_width, random_height, spawn_pos.z);
        //Access the obstacle script
        Ring_Movement script = instanciated_rings[random_pick].GetComponent<Ring_Movement>();

        //Check if pick is from pool
        if (script.GetPoolSpawnFlag())
        {
            Debug.Log("Pick is from pool.");
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
    public void SetMovementSpeed(float speed)
    {
        for (int i = 0; i < instanciated_rings.Length; i++)
        {
            Ring_Movement script = instanciated_rings[i].GetComponent<Ring_Movement>();
            script.SetMovementSpeed(speed);
        }
    }

}
