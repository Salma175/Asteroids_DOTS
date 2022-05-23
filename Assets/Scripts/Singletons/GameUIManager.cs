using System;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private GameStates currentState = GameStates.None;

    private int score = 0;
    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }

    private void Start()
    {
        ResetUI();
    }

    public void EnableInGameUI(GameStates gameState) {
        if (currentState != gameState)
        {
            canvas.SetActive(true);
            currentState = gameState;
        }
    }

    public void DisableInGameUI(GameStates gameState)
    {
        if (currentState != gameState)
        {
            Debug.Log("Reset UI");
            ResetUI();
            currentState = gameState;
        }
    }

    public void UpdateScore() {
        score += 1;
        scoreText.SetText(GetFormattedScore(score));
    }

    private string GetFormattedScore(int score) {
        return String.Format("Score : {0}",score);
    }

    private void ResetUI()
    {
        score = 0;
        scoreText.SetText(GetFormattedScore(score));
        canvas.SetActive(false);
    }
}
