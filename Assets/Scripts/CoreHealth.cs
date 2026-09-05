using UnityEngine;
using UnityEngine.UI;

public class CoreHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private GameObject mainMenuPanel;

    void Start()
    {
        currentHealth = maxHealth;
        if (deathScreenPanel != null) deathScreenPanel.SetActive(false);
        UpdateHealthUI();
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
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
        healthSlider.value = currentHealth / maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthSlider.value = currentHealth / maxHealth;
        Debug.Log($"Core Healed! Health: {currentHealth}/{maxHealth}");
    }
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    private void GameOver()
    {
        Debug.Log("Core Destroyed! Game Over!");
        if (gamePanel != null) gamePanel.SetActive(false);
        if (deathScreenPanel != null) deathScreenPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}