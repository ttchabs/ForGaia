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
    public string titleScreen;
    public string expositionScreen;
    public string level1;
    public string level2;
    public string level3;

    public void Awake()
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

    public void LoadStart()
    {
        SceneManager.LoadScene("");
    }

    public void LoadExposition()
    {

    }
}
