using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private string nameLevel;
     [SerializeField] private GameObject windowHomeMenu;
     [SerializeField] private GameObject windowOptions;

    public void Play()
    {
        SceneManager.LoadScene(nameLevel);
    }

  
    public void OpenOptions()
    {
        windowHomeMenu.SetActive(false);
        windowOptions.SetActive(true);

    }

     public void CloseOptions()
    {
        windowHomeMenu.SetActive(true);
        windowOptions.SetActive(false);
    }

     public void QuitGame()
    {
        Application.Quit();
    }
}
