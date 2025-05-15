using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource audioSource;
    public GameObject Cursorstuff;
    private bool isPaused = false;
    void Start()
    {
        pauseMenuUI.SetActive(false);
        audioSource.Play();
        Cursorstuff.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursorstuff.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        audioSource.volume = 1;
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursorstuff.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource.volume = 0;
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
