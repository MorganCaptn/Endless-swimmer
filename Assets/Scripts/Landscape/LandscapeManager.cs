using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour
{
    public GameObject spawn_line;
    public GameObject next_element_line;
    public float next_landscape_cooldown;
    public GameObject[] landscape_elements;
    private GameObject[] instanciated_landscapes_left;
    private GameObject[] instanciated_landscapes_right;
    public float landscape_left_offset = -6.5f;
    public float landscape_right_offset = 14.5f;


    private bool spawned_left = false;
    private bool spawned_right = false;
    private Vector3 spawn_pos;

    private Vector3 pool_position = new Vector3(40, 40, 0);
    private bool next_landscape_element = true;

    // Start is called before the first frame update
    void Start()
    {
        CreateLandscapePool();
        spawn_pos = spawn_line.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(next_landscape_element)
        {
            if (!spawned_left)
            {
                spawned_left = SpawnLandscape(instanciated_landscapes_left, landscape_left_offset);
            }
            if (!spawned_right)
            {
                spawned_right = SpawnLandscape(instanciated_landscapes_right, landscape_right_offset);
            }
            if (spawned_left && spawned_right)
            {
                StartCoroutine(CooldownTimer());
                
            }
            

        }



    }

    private IEnumerator CooldownTimer()
    {
        //Debug.Log("TIMER!!");
        next_landscape_element = false;
        // then wait for it's cooldown.
        yield return new WaitForSeconds(next_landscape_cooldown);
        next_landscape_element = true;
        spawned_left = false;
        spawned_right = false;
    }

    void CreateLandscapePool()
    {
        //creating each obstacle twice, for left and right
        instanciated_landscapes_left = new GameObject[landscape_elements.Length];
        instanciated_landscapes_right = new GameObject[landscape_elements.Length];

        for (int i = 0; i < landscape_elements.Length; i++)
        {
            instanciated_landscapes_left[i] = Instantiate(landscape_elements[i], pool_position, Quaternion.identity);
            instanciated_landscapes_right[i] = Instantiate(landscape_elements[i], pool_position, Quaternion.identity);
            instanciated_landscapes_left[i].SetActive(false);
            instanciated_landscapes_right[i].SetActive(false);

        }

    }

    public bool SpawnLandscape(GameObject[] instanciated_landscapes, float offset)
    {
        bool landscape_spawned = false;
        Vector3 landscape_spawn_pos = new Vector3(offset, 6.5f, spawn_pos.z);
          
        


        //get a random number
        int random_pick = Random.Range(0, landscape_elements.Length);
        
        //Access the obstacle script
        Movement script = instanciated_landscapes[random_pick].GetComponent<Movement>();
        
        if (!instanciated_landscapes[random_pick].activeSelf)
        {
            //Debug.Log("Pick is from pool.");
            instanciated_landscapes[random_pick].SetActive(true);
            instanciated_landscapes[random_pick].transform.position = landscape_spawn_pos;
            script.SetMovement(true);
            script.SetPoolSpawnFlag(false);
            landscape_spawned = true;
        }
        /*
        //Check if pick is from pool
        if (script.GetPoolSpawnFlag())
        {
            //Debug.Log("Pick is from pool.");
            instanciated_landscapes[random_pick].transform.position = landscape_spawn_pos;
            script.SetMovement(true);
            script.SetPoolSpawnFlag(false);
            landscape_spawned = true;
        }
        //Check if obstacle can already be respawned
        else if (script.GetRespawnFlag())
        {
            //Debug.Log("Pick can be respawned");
            instanciated_landscapes[random_pick].transform.position = landscape_spawn_pos;
            script.SetMovement(true);
            script.SetRespawnFlag(false);
            landscape_spawned = true;
        }

        */
        //Try another pick
        else
        {
            //Debug.Log("Could not find a suitable pick.");
            landscape_spawned = false;
        }
        return landscape_spawned;

    }


    public void SetMovement(bool move)
    {
        for (int i = 0; i < instanciated_landscapes_left.Length; i++)
        {
            Movement script = instanciated_landscapes_left[i].GetComponent<Movement>();
            script.SetMovement(move);
        }

        for (int i = 0; i < instanciated_landscapes_right.Length; i++)
        {
            Movement script = instanciated_landscapes_right[i].GetComponent<Movement>();
            script.SetMovement(move);
        }
    }
    public void SetMovementSpeed(float speed)
    {
        for (int i = 0; i < instanciated_landscapes_left.Length; i++)
        {
            Movement script = instanciated_landscapes_left[i].GetComponent<Movement>();
            script.SetMovementSpeed(speed);
        }

        for (int i = 0; i < instanciated_landscapes_right.Length; i++)
        {
            Movement script = instanciated_landscapes_right[i].GetComponent<Movement>();
            script.SetMovementSpeed(speed);
        }
    }
}