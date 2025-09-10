using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    private Health _health;

    void Start()
    {
        _health = GetComponent<Health>();

        if (_health != null)
            _health.OnDie += HandleDeath;
    }

    void HandleDeath()
    {
        SceneManager.LoadScene("GameOver");
    }
}