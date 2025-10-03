using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Opciones")]
    public GameObject pauseUI;
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Start()
    {
        if (pauseUI) pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("[PauseMenu] Juego pausado");
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseUI) pauseUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
    }

    public void ResumeGame(bool keepCursorVisible = false)
    {
        Debug.Log("[PauseMenu] Reanudando juego");
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseUI) pauseUI.SetActive(false);

        if (keepCursorVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (allAudioSources != null)
        {
            foreach (AudioSource audio in allAudioSources)
            {
                audio.UnPause();
            }
        }
    }

    public void LoadMainMenu()
    {
        Debug.Log("[PauseMenu] Cargando menú principal");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}