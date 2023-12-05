using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string LEVEL_1_SCENE_BUILD_NAME = "Level 1";
    public void StartGame()
    {
        SceneManager.LoadScene(LEVEL_1_SCENE_BUILD_NAME);
    }
}
