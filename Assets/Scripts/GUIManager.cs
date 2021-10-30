using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private Text score_text;
    public GameObject score_element;
    // Start is called before the first frame update
    void Start()
    {
        score_text = score_element.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintScore(int score)
    {
        score_text.text = score.ToString();
    }
}
