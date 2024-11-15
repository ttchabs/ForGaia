using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [Header("PauseFunction")]
    public GameObject pausePanel;
    public GameObject ControlsPanel;
    public bool isPaused;
    public Button Resume;
    public Button Quit;

    public void Awake()
    {
        if (instance == null)
        {
        instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        Resume.onClick.AddListener(ResumeGame);
        Quit.onClick.AddListener(QuitGame);
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        FirstPersonControls.Instance.playerInput.Disable();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        FirstPersonControls.Instance.playerInput.Enable();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
