using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private Text score_text;
    private Text level_text;
    public GameObject score_element;
    public GameObject level_element;
    // Start is called before the first frame update
    void Start()
    {
        score_text = score_element.GetComponent<Text>();
        level_text = level_element.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintScore(int score)
    {
        string current_score = "Score: " + score.ToString();
        score_text.text = current_score;
    }

    public void PrintLevel(int level)
    {
        string current_score = "Level: " + level.ToString();
        level_text.text = current_score;
    }
}
