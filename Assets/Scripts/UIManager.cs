using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text gameOverPointText;
    [SerializeField] private TMP_Text timerText;

    public Button startButton;
    public Button restartButton;

    [SerializeField] private GameObject startPanel;

    [SerializeField] private GameObject inGamePanel;

    [SerializeField] private GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void StartGame()
    {
        GameManager.GM.isGamePlay = true;
        GameManager.GM.StartGame();
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void GameOver()
    {
        GameManager.GM.isGamePlay = false;
        gameOverPointText.text = GameManager.GM.pointAll.ToString();
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void UpdatePoint(int _point)
    {
        pointText.text = _point.ToString();
    }

    public void UpdateTimer(float time)
    {
        
        if (time > 5f)
            time = Mathf.Floor(time);
        else if(time > 0)
        {
            time = Mathf.Round(time * 100.0f) * 0.01f;
        }
        else
        {
            time = 0;
        }

        timerText.text = time.ToString();
    }

    public void RestartGame()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
