using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource MenuMusic; // Ссылка на источник музыки

    private bool isMuted = false;
    private float lastVolume = 1f;

    private void Start()
    {
        // Загружаем сохраненную громкость (1.0f по умолчанию)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        SetVolume(savedVolume);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }

    // Изменение громкости (для Slider)
    public void SetVolume(float volume)
    {
        // 1. Изменяем общую громкость игры
        AudioListener.volume = volume;

        // 2. Если хотите отдельно управлять громкостью музыки (если она есть)
        if (MenuMusic != null)
        {
            MenuMusic.volume = volume;
        }

        // Сохраняем значение
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

        if (volume > 0)
        {
            lastVolume = volume;
            isMuted = false;
        }
        else
        {
            isMuted = true;
        }
    }

    // Включение / Выключение звука (Mute)
    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            if (volumeSlider != null) lastVolume = volumeSlider.value;
            SetVolume(0f);
            if (volumeSlider != null) volumeSlider.value = 0f;
        }
        else
        {
            SetVolume(lastVolume);
            if (volumeSlider != null) volumeSlider.value = lastVolume;
        }
    }

    // Метод для кнопки «Назад»
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }
}