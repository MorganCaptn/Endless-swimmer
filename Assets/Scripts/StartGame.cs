using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public void Scene1()
    {
        Debug.Log("Load Scene");
        SceneManager.LoadScene("SampleScene");
    }
    
}
