using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject AudioPanel;
    public GameObject DisplayPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void OnClickReturn()
    {
        ShowMainMenu();
    }

    public void OnClickOptions()
    {
        HideAllMenus();
        if (OptionsMenu != null)
        {
            OptionsMenu.SetActive(true);
        }
        ShowAudioSettings();
    }
    public void ShowAudioSettings()
    {
      //  Debug.Log(" Mostrando ajustes de Audio");
        if (AudioPanel != null) AudioPanel.SetActive(true);
        if (DisplayPanel != null) DisplayPanel.SetActive(false);
    }

    public void ShowDisplaySettings()
    {
      //  Debug.Log(" Mostrando ajustes de Pantalla");
        if (AudioPanel != null) AudioPanel.SetActive(false);
        if (DisplayPanel != null) DisplayPanel.SetActive(true);
    }

    private void ShowMainMenu()
    {
       // Debug.Log("Mostrando MainMenu");
        HideAllMenus();
        if (MainMenu != null)
        {
            MainMenu.SetActive(true);
        }
    }

    private void HideAllMenus()
    {
        if (MainMenu != null)
        {
            MainMenu.SetActive(false);
        }
        if (OptionsMenu != null)
        {
            OptionsMenu.SetActive(false);
        }
    }
}
