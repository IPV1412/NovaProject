using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage objectiveImage;          
    public TMP_Text objectiveText;            

    [Header("Settings")]
    public string enemyTag = "Enemy";     
    public string missionName = "Derrota a todos los enemigos";

    [Header("References")]
    public SceneNavigation navigation;         

    private int totalEnemies;
    private int currentEnemies;
    private bool objectiveCompleted;

    void Start()
    {
        totalEnemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;
        currentEnemies = totalEnemies;

        UpdateObjectiveText();
    }

    void Update()
    {
        currentEnemies = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        UpdateObjectiveText();

        if (!objectiveCompleted && currentEnemies <= 0)
        {
            objectiveCompleted = true;
            if (navigation != null)
            {
                navigation.Win();
            }
        }
    }

    void UpdateObjectiveText()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"{missionName} {totalEnemies - currentEnemies}/{totalEnemies}";
        }
    }
}