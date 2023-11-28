using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    bool isPaused;
    [SerializeField] OfficeExplorationController officeExplorationController;

    public void OnPauseAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPaused = !isPaused;
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

    }

    public void OnRestartAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Restart();
        }
    }

    void Restart()
    {
        Debug.Log("Restarted");
        officeExplorationController.DestroyThyself();
        DestroyThyself();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
