using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;     // Панель с кнопками главного меню
    [SerializeField] private GameObject settingsPanel; // Панель настроек

    private void Start()
    {
        // При старте показываем главное меню и скрываем настройки
        if (menuPanel != null) menuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    // Метод для открытия настроек (для кнопки "Настройки")
    public void OpenSettings()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Метод для закрытия настроек (для кнопки "Назад")
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}