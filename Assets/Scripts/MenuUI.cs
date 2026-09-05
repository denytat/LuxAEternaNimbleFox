using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;


    private void Start()
    {
        // При запуске гарантируем, что окно настроек скрыто
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
   public void OpenSettings(bool isOpen)
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(isOpen);
        }
    }
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

}
