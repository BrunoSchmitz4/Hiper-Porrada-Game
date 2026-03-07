using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text comboText;
    public Text scoreText;

    private Double combo = 0;
    private Double score = 0;
    private Double previewCombo = 0;

    // Eventos para notificar outras classes
    public event Action<Double> OnComboChanged;
    public event Action<Double> OnScoreChanged;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start() { UpdateUI(); }

    public void AddCombo()
    {
        combo += 1;

        // Toca som a cada 10 combos
        if ((combo - previewCombo == 10))
        {
            previewCombo = combo;
            AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerAddCombo);
        }

        //score += 10 * Math.Ceiling(combo / 10);

        UpdateUI();

        // Notifica listeners (Observer Pattern)
        OnComboChanged?.Invoke(combo);
        OnScoreChanged?.Invoke(score);
    }

    public void AddScore(int points)
    {
        double comboMultiplier = Math.Ceiling(combo / 10);
        if (comboMultiplier < 1) comboMultiplier = 1;

        score += points * comboMultiplier;
        UpdateUI();
    }

    public void DelCombo()
    {
        //AudioManager.instance?.PlaySFX(AudioManager.instance.loseCombo);
        AudioManager.instance?.PlaySFXAudioMixer(SFX.PlayerLoseCombo);

        combo = 0;
        previewCombo = 0;
        UpdateUI();
        OnComboChanged?.Invoke(combo);
    }

    private void UpdateUI()
    {
        if (comboText != null)
            comboText.text = combo.ToString() + " x";

        if (scoreText != null)
            scoreText.text = score.ToString() + " pts";
    }

    public Double GetScore() => score;
    public Double GetCombo() => combo;

    public void ResetScore()
    {
        combo = 0;
        score = 0;
        previewCombo = 0;
        UpdateUI();
    }
}