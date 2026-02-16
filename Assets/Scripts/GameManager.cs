using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configurações")]
    public bool isPaused = false;
    public bool isGameOver = false;

    private void Awake()
    {
        // Implementação Singleton com proteção
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Pausa com ESC (exemplo)
        //if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        //{
        //    TogglePause();
        //}
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        // Implementar aqui um menu de pausa (falar com o pedro antes)
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        // Toca som de morte
        AudioManager.instance?.PlaySFX(AudioManager.instance.death);

        Debug.Log("Game Over! Score: " + ScoreManager.instance.GetScore());

        // Implementar aqui a lógica para mostrar a tela de Game Over, como um painel com opções de reiniciar ou voltar ao menu (falar com o pedro antes)
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isPaused = false;

        ScoreManager.instance?.ResetScore();

        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isPaused = false;
        SceneManager.LoadScene(sceneName);
    }


}


// Implementar um InputManager yeeeeeeeeeeeeeeeeee
