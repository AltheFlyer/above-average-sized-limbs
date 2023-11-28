using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    int MAIN_MENU_BUILD_INDEX = 0;
    bool isPaused;
    [SerializeField] OfficeExplorationController officeExplorationController;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public void OnPauseAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause(!isPaused);
            pauseScreen.SetActive(isPaused);
        }
    }

    private void TogglePause(bool shouldPause)
    {
        isPaused = shouldPause;
        if (isPaused)
        {
            Time.timeScale = 0;
            Debug.Log("Paused");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Unpaused");
        }
    }

    public void OnRestartAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Restart();
        }
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGameState()
    {
        gameOverScreen.SetActive(false);
        TogglePause(false);
        officeExplorationController.DestroyThyself();
        DestroyThyself();
    }

    public void MainMenuClicked()
    {
        ResetGameState();
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }

    public void OnGameOver()
    {
        Debug.Log("Game Over!");
        TogglePause(true);
        gameOverScreen.SetActive(true);
    }
}
