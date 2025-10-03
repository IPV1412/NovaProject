using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneNavigation : MonoBehaviour
{
    [Header("Escenas")]
    public string gameSceneName = "Game";
    public string creditsSceneName = "Credits";
    public string winSceneName = "Win";
    public string loadingSceneName = "LoadingScene";
    public string mainMenuSceneName = "MainMenu";

    public void OnClickPlay()
    {
        LoadSceneWithDelay(gameSceneName, 0f);
    }

    public void OnClickCredits()
    {
        LoadSceneWithDelay(creditsSceneName, 0.1f);
    }
    
    public void Win()
    {
        LoadSceneWithDelay(winSceneName, 0.1f);
    }

    public void OnClickLoad()
    {
        LoadSceneWithDelay(loadingSceneName, 0.2f);
    }

    public void OnClickMenu()
    {
        LoadSceneWithDelay(mainMenuSceneName, 0.5f);
    }

    private void LoadSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(DelayLoad(sceneName, delay));
    }

    private IEnumerator DelayLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
