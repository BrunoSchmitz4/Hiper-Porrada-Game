using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

void Update() // Update = Verifica a todo momento a condição que está dentro dessa função
    {
        if (Keyboard.current.anyKey.isPressed) //Se qualquer tecla do teclado for pressionada ->
        {
            InputSystem.ResetDevice(Keyboard.current); //Reseta o input, para quando for redirecionado para o menu, não existir conflitos
            
            SceneManager.LoadScene(nextSceneName); //Carrega a scene do menu principal
        }
    }
}

