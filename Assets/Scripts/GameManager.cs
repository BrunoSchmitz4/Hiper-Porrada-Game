using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static AudioManager audioManager;

    public void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        Instance = this;
    }
}


// Implementar um InputManager yeeeeeeeeeeeeeeeeee
