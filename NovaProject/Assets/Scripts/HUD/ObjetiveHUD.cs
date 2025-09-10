using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage objectiveImage;          
    public TMP_Text objectiveText;            

    [Header("Settings")]
    public string enemyTag = "Enemy";     
    public string missionName = "Derrota a todas las torretas";

    [Header("References")]
    public SceneNavigation navigation;         

    private int totalEnemies;
    private int currentEnemies;
    private bool objectiveCompleted;

    void Start()
    {
        totalEnemies = FindObjectsByType<TurretAI>(FindObjectsSortMode.None)
            .Count(t => t.CompareTag(enemyTag));
        currentEnemies = totalEnemies;

        UpdateObjectiveText();
    }

    void Update()
    {
        currentEnemies = FindObjectsByType<TurretAI>(FindObjectsSortMode.None)
            .Count(t => t.CompareTag(enemyTag));

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