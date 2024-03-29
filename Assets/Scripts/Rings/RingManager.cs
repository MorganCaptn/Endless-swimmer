using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public GameObject[] rings;
    public GameObject[] super_rings;

    public GameObject spawn_line;

    public float ring_cooldown = 5.0f;
    public float ring_spawn_probability = 1.0f;

    public float super_ring_cooldown = 10.0f;
    public float super_ring_spawn_probability = 1.0f;

    private GameObject[] instanciated_rings;
    private GameObject[] instanciated_super_rings;
    private bool ring_spawn_cooldown = false;
    private bool super_ring_spawn_cooldown = false;

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
        float ring_probability = Random.Range(0.0f, 1.0f);
        float super_ring_probability = Random.Range(0.0f, 1.0f);
        float threshold = 1.0f - ring_spawn_probability;

     

        if (ring_probability >= threshold && !ring_spawn_cooldown)
        {
            SpawnRing();
        }

        if (super_ring_probability >= threshold && !super_ring_spawn_cooldown)
        {
            SpawnSuperRing();
        }

        //Add respawn cooldown
        if (!ring_spawn_cooldown)
        {
            StartCoroutine(RingCooldownTimer());
        }
        
        if (!super_ring_spawn_cooldown)
        {
            StartCoroutine(SuperRingCooldownTimer());
        }



    }


    void CreateRingPool()
    {
        //assuming each obstacle is available twice in the world
        instanciated_rings = new GameObject[rings.Length * 2];
        instanciated_super_rings = new GameObject[super_rings.Length * 2];

        for (int i = 0; i < rings.Length; i++)
        {
            instanciated_rings[i * 2] = Instantiate(rings[i], pool_position, Quaternion.identity);
            instanciated_rings[(i * 2) + 1] = Instantiate(rings[i], pool_position, Quaternion.identity);
            instanciated_rings[i * 2].SetActive(false);
            instanciated_rings[(i * 2) + 1].SetActive(false);
        }

        for (int i = 0; i < super_rings.Length; i++)
        {
            instanciated_super_rings[i * 2] = Instantiate(super_rings[i], pool_position, Quaternion.identity);
            instanciated_super_rings[(i * 2) + 1] = Instantiate(super_rings[i], pool_position, Quaternion.identity);
            instanciated_super_rings[i * 2].SetActive(false);
            instanciated_super_rings[(i * 2) + 1].SetActive(false);
        }

    }

    public void ReturnRingToPool()
    {

    }

    private IEnumerator RingCooldownTimer()
    {
        ring_spawn_cooldown = true;
        // then wait for it's cooldown.
        yield return new WaitForSeconds(ring_cooldown);
        ring_spawn_cooldown = false;
    }

    private IEnumerator SuperRingCooldownTimer()
    {
        super_ring_spawn_cooldown = true;
        // then wait for it's cooldown.
        yield return new WaitForSeconds(super_ring_cooldown);
        super_ring_spawn_cooldown = false;
    }

    public void SpawnRing()
    {

        //TODO: Fix ring appearance behaviour
        //get random numbers
        int random_pick = Random.Range(0, instanciated_rings.Length);
        float random_height = Random.Range(-1.8f, 3.8f);
        float random_width = Random.Range(-3.8f, 3.8f);
        Vector3 instance_pos = new Vector3(random_width, random_height, spawn_pos.z);
        //Access the obstacle script
        Movement script = instanciated_rings[random_pick].GetComponent<Movement>();


        if (!instanciated_rings[random_pick].activeSelf)
        {
            //Debug.Log("Pick is from pool.");
            instanciated_rings[random_pick].SetActive(true);
            instanciated_rings[random_pick].transform.position = instance_pos;
            script.SetMovement(true);

        }

        //Try another pick
        else
        {
           // Debug.Log("Could not find a suitable pick.");
        }

    }

    public void SpawnSuperRing()
    {

        //get random numbers
        int random_pick = Random.Range(0, instanciated_super_rings.Length);
        //float random_height = Random.Range(-1.8f, 3.8f);
        float height = 6f;
        float random_width = Random.Range(-3.8f, 3.8f);
        Vector3 instance_pos = new Vector3(random_width, height, spawn_pos.z);
        //Access the obstacle script
        Movement script = instanciated_super_rings[random_pick].GetComponent<Movement>();


        if (!instanciated_super_rings[random_pick].activeSelf)
        {
            //Debug.Log("Pick is from pool.");
            instanciated_super_rings[random_pick].SetActive(true);
            instanciated_super_rings[random_pick].transform.position = instance_pos;
            script.SetMovement(true);
        }

        else
        {
           // Debug.Log("Could not find a suitable pick.");
        }

    }

    public void SetMovement(bool move)
    {
        for (int i = 0; i < instanciated_rings.Length; i++)
        {
            Movement script = instanciated_rings[i].GetComponent<Movement>();
            script.SetMovement(move);
        }

        for (int i = 0; i < instanciated_super_rings.Length; i++)
        {
            Movement script = instanciated_super_rings[i].GetComponent<Movement>();
            script.SetMovement(move);
        }
    }
    public void SetMovementSpeed(float speed)
    {
        for (int i = 0; i < instanciated_rings.Length; i++)
        {
            Movement script = instanciated_rings[i].GetComponent<Movement>();
            script.SetMovementSpeed(speed);
        }

        for (int i = 0; i < instanciated_super_rings.Length; i++)
        {
            Movement script = instanciated_super_rings[i].GetComponent<Movement>();
            script.SetMovementSpeed(speed);
        }
    }

}
