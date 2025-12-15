using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text comboText;
    public Text scoreText;

    int combo = 0;
    int score = 0;

    private void Awake()
    {
        instance = this;
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
        score += combo * 10;
        scoreText.text = score.ToString() + " pts";
    }
    public void DelCombo()
    {
        combo = 0;
        comboText.text = combo.ToString() + " x";
    }
}
