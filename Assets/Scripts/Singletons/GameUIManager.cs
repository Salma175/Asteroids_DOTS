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

    public void EnableInGameUI() {
        canvas.SetActive(true);
    }
    public void DisableInGameUI()
    {
        ResetUI();
    }
    public void UpdateScore(int score) {
        scoreText.SetText(GetFormattedScore(score));
    }

    private string GetFormattedScore(int score) {
        return String.Format("Score : {0}",score);
    }

    private void ResetUI()
    {
        scoreText.SetText(GetFormattedScore(0));
        canvas.SetActive(false);
    }
}
