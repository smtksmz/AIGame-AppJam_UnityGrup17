using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIElements : MonoBehaviour
{

    public Slider actionSlider; // Refer to the slider to be toggled
    public Button toggleButton; // Refer to the button that triggers the toggle
    public AudioMixer audioMixer; // Refer to the AudioMixer
    public Slider volumeSlider; // Refer to the UI Slider
    public void LoadNextScene()
    {
        // Mevcut sahnenin indeksini al
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Bir sonraki sahneye geç
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }



    void Start()
    {
        toggleButton.onClick.AddListener(ToggleButtonVisibility);
        // Load the volume level from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = savedVolume;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20);
        }
        else
        {
            volumeSlider.value = 1f;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(1f) * 20);
        }

        // Add a listener to call the method when the slider value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    private void ToggleButtonVisibility()
    {
        // Toggle the visibility and interactability of the actionButton
        bool isActive = actionSlider.gameObject.activeSelf;
        actionSlider.gameObject.SetActive(!isActive);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
