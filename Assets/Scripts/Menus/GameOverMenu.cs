using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    GameObject menu;

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TurnsScript>().onNewTurn += CheckGameDone;
    }

    void CheckGameDone()
    {
        if (MainBalancing.turn > 200)
        {
            EscapeLevelHandler.pauseMenuOpen = true;
            menu.SetActive(true);
        }
    }

    
}
