using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    int LEVEL_1_SCENE_BUILD_INDEX = 2;
    public void StartGame()
    {
        SceneManager.LoadScene(LEVEL_1_SCENE_BUILD_INDEX);
    }
}
