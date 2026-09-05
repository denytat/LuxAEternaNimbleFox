using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
   

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;


    private void Start()
    {
       
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
