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


    }

    public void Start()
    {
        Resume.onClick.AddListener(ResumeGame);
        Quit.onClick.AddListener(QuitGame);        
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        UIManager.Instance.DeactivateAllUI();
        AudioListener.pause = true;
        FirstPersonControls.Instance.playerInput.Player.Disable();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        UIManager.Instance.ReactivateAllUI();
        FirstPersonControls.Instance.playerInput.Player.Enable();
        AudioListener.pause = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
