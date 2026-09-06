using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("UI Panels")]
    [SerializeField] private GameObject gamePanel;        // Игровой HUD (текст волны, HP)
    [SerializeField] private GameObject deathScreenPanel; // Панель экрана смерти
    [SerializeField] private GameObject mainMenuPanel;   // Главное меню

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Скрываем экран смерти при запуске
        if (deathScreenPanel != null) deathScreenPanel.SetActive(false);
    }

    private void Update()
    {
        // Обновление счетчика волн
        if (WaveManager.Instance != null && waveText != null)
        {
            waveText.text = $"Wave: {WaveManager.Instance.currentWave}";
        }
    }

    // --- УПРАВЛЕНИЕ ЭКРАНОМ СМЕРТИ ---

    public void ShowDeathScreen()
    {
        if (gamePanel != null) gamePanel.SetActive(false);
        if (deathScreenPanel != null) deathScreenPanel.SetActive(true);

        Time.timeScale = 0f; // Пауза игры при поражении
    }

    // Кнопка 1: Retry
    public void Retry()
    {
        Time.timeScale = 1f; // Снимаем паузу
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Кнопка 2: Main Menu
    public void OpenMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene"); // Загружаем главное меню
    }
}