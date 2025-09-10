using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string url = "https://www.fiftysounds.com/es/";

    public void OpenURL()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}