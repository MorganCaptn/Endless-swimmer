using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperRing : MonoBehaviour
{
    public int point_value = 150;
    private bool collected = false;
    public Material defeat_material;
    private Material default_material;
    private MeshRenderer my_renderer;
    private LineRenderer line_renderer;
    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<MeshRenderer>();
        default_material = my_renderer.material;
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("SuperRing collided with player!");
            collected = true;
            this.Disappear();

        }
        if (other.gameObject.tag == "Respawn")
        {

            //Change back to original appearance
            Debug.Log("SuperRing collided with respawn line!");
            collected = false;
            this.Appear();
            //Assuming this script always has a parent with the ring movement script attached
            Ring_Movement script = gameObject.transform.parent.gameObject.GetComponent<Ring_Movement>();
            script.SetRespawnFlag(true);
            script.SetMovement(false);
            gameObject.SetActive(false);

        }
    }

    public void Disappear()
    {
        my_renderer.material = defeat_material;
        //TODO: Let the ring disappear... in a cool way

    }

    public void Appear()
    {
        collected = false;
        my_renderer.material = default_material;


    }

    public bool GetCollectStatus()
    {
        return collected;
    }

    public int GetPoints()
    {
        return point_value;
    }
}
