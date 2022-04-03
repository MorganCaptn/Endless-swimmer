using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    private Text score_text;
    private Text level_text;
    private Text score_text_game_over;
    private Text level_text_game_over;
    public GameObject score_element;
    public GameObject level_element;
    public GameObject score_element_game_over;
    public GameObject level_element_game_over;
    public GameObject GUI_canvas;
    public GameObject game_over_screen;

    // Start is called before the first frame update
    void Start()
    {
        score_text = score_element.GetComponent<Text>();
        level_text = level_element.GetComponent<Text>();
        score_text_game_over = score_element_game_over.GetComponent<Text>();
        level_text_game_over = level_element_game_over.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateGUI(bool activate)
    {
        GUI_canvas.SetActive(activate);
    }

    public void ActivateGameOverScreen(bool activate)
    {
        game_over_screen.SetActive(activate);
    }

    public void PrintScore(int score)
    {
        string current_score = "Score: " + score.ToString();
        string current_score_game_over = "Your Score: " + score.ToString();
        score_text.text = current_score;
        score_text_game_over.text = current_score_game_over;
    }

    public void PrintLevel(int level)
    {
        string current_level = "Level: " + level.ToString();
        string current_level_game_over = "Your level: " + level.ToString();
        level_text.text = current_level;
        level_text_game_over.text = current_level_game_over;
    }
}
