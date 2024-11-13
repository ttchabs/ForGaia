using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager managerInstance;

    [Header("PAUSE MENU:")]
    public GameObject pausePanel;
    public GameObject ControlsPanel;
    public bool isPaused = false;
    public Button Resume;
    public Button Quit;

    [Header("SCENES:")]
    public GameManagerScriptable managerConfigs;

    public void Start()
    {
        if (managerInstance == null)
        {
            managerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void ReloadGame(string currentSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneName);
    }

    public IEnumerator LoadGame()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(managerConfigs.SceneNames[0]);

        yield return null;
    }
}
