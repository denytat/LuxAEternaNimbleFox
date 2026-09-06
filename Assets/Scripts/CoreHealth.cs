using UnityEngine;
using UnityEngine.UI;

public class CoreHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject playerCore;
    [SerializeField] private GameObject deathScreenPanel;

    private void Start()
    {
        ResetHealth();
        if (deathScreenPanel != null) deathScreenPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
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

        UpdateHealthUI(); // Используем только безопасный метод!

        Debug.Log($"Core Damaged! Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            GameOver();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI(); // Используем только безопасный метод!

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
        if (playerCore != null) playerCore.SetActive(false);
        if (deathScreenPanel != null) deathScreenPanel.SetActive(true);

        Time.timeScale = 0f; // Ставим игру на паузу
    }
}