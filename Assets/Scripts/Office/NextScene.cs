using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerManager>().ClimbLadder();
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3.0f);
        DontDestroyOnLoad(FindObjectOfType<PlayerManager>());
        DontDestroyOnLoad(FindObjectOfType<PlayerInventory>());
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }
}