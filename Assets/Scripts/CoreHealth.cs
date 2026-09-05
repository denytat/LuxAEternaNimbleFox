using UnityEngine;

public class CoreHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0f, currentHealth);
        Debug.Log($"Core Damaged! Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            GameOver();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Core Healed! Health: {currentHealth}/{maxHealth}");
    }

    private void GameOver()
    {
        Debug.Log("Core Destroyed! Game Over!");
    }
}