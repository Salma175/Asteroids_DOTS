using System;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        EventManager.ScoreUpdateEvent += UpdateScore;
        EventManager.HandleGameUIEvent += HandleInGameUI;
        ResetUI();
    }

    public void HandleInGameUI(bool m_enable)
    {
        if (m_enable) { EnableInGameUI(); } else { DisableInGameUI(); }
    }
    public void EnableInGameUI()
    {
        canvas.SetActive(true);
    }
    public void DisableInGameUI()
    {
        ResetUI();
    }
    public void UpdateScore(int score)
    {
        scoreText.SetText(GetFormattedScore(score));
    }

    private string GetFormattedScore(int score)
    {
        return String.Format("Score : {0}", score);
    }

    private void ResetUI()
    {
        scoreText.SetText(GetFormattedScore(0));
        canvas.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.ScoreUpdateEvent -= UpdateScore;
        EventManager.HandleGameUIEvent -= HandleInGameUI;
    }
}
