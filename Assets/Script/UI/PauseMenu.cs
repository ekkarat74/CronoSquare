using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Include this namespace for scene management

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private bool isPaused;
    [SerializeField] private GameObject PausePanel;
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        isPaused = false;
    }

    // Method to load the main menu scene
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Ensure time is resumed when returning to the main menu
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }

    // Method to load the PlayGame scene
    public void GoToPlayGame()
    {
        Time.timeScale = 1; // Ensure time is resumed when returning to the PlayGame scene
        SceneManager.LoadScene("PlayGame"); // Replace "PlayGame" with the name of your PlayGame scene
    }
}