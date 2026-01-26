using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text comboText;
    public Text scoreText;


    // Combo = quantos inimigos eliminados em sequência (atualizar) sem tomar dano
    Double combo = 0;
    Double score = 0;
    Double previewCombo = 0;

    AudioManager audioManager;


    private void Awake()
    {
        instance = this;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        scoreText.text = score.ToString() + " pts";
        comboText.text = combo.ToString() + " x";
    }

    public void AddCombo()
    {
        combo += 1;
        comboText.text = combo.ToString() + " x";
        if((combo - previewCombo == 10))
        {
            previewCombo = combo;
            audioManager.PlaySFX(audioManager.addCombo);
        }
        score += 10 * Math.Ceiling(combo / 10);
        scoreText.text = score.ToString() + " pts";
    }
    public void DelCombo()
    {
        audioManager.PlaySFX(audioManager.loseCombo);
        combo = 0;
        comboText.text = combo.ToString() + " x";
    }
}
