using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    [Header("Settings")]
    public string sceneToLoad;
    public float fakeLoadTime = 3f; // Duración artificial del progreso

    private void Start()
    {
        Debug.Log("[LoadingManager] Iniciando carga de escena con simulación visual: " + sceneToLoad);
        StartCoroutine(LoadSceneWithProgress(sceneToLoad));
    }

    IEnumerator LoadSceneWithProgress(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;

        float elapsedTime = 0f;
        float visualProgress = 0f;

        while (!asyncOp.isDone)
        {
            float targetProgress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            
            if (elapsedTime < fakeLoadTime)
            {
                elapsedTime += Time.deltaTime;
                visualProgress = Mathf.Lerp(0f, targetProgress, elapsedTime / fakeLoadTime);
            }
            else
            {
                visualProgress = targetProgress;
            }

            loadingBar.value = visualProgress;
            if (loadingText != null)
                loadingText.text = Mathf.RoundToInt(visualProgress * 100f) + "%";

            Debug.Log($"[LoadingManager] Visual: {Mathf.RoundToInt(visualProgress * 100f)}% - Real: {Mathf.RoundToInt(targetProgress * 100f)}%");
            
            if (targetProgress >= 0.9f && elapsedTime >= fakeLoadTime)
            {
                Debug.Log("[LoadingManager] Escena lista y tiempo cumplido, activando...");
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

