using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float time_delay=10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time_delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
