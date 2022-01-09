using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public int point_value = 20;
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Ring collided with player!");
            collected = true;

        }
    }

    public void Disappear()
    {
        // Let the ring disappear... in a cool way
    }

    public void ResetCollectStatus()
    {
        collected = false;
    }

    public bool GetCollectStatus()
    {
        return collected;
    }

    int GetPoints()
    {
        return point_value;
    }

}
