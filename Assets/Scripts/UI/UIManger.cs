using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera (assign in the Inspector)
    public float rotationSpeed = 5f; // Speed at which the camera rotates
    private bool isRotating = false; // Track if the camera is currently rotating
    public GameObject[] UIElements;
    public GameObject initialButton;
    public TextScroll textScroll;
    public float scrollLength;
    public GameObject panel;
    public GameObject tree;


    public void Start()
    {
        panel.SetActive(false);
    }
    // Method to rotate the camera left by 90 degrees
    public void RotateCameraLeftBy90Degrees()
    {
        if (!isRotating) // Prevent triggering multiple rotations simultaneously
        {
            StartCoroutine(RotateCameraCoroutine(10f));
        }
    }
// Coroutine to smoothly rotate the camera
    private IEnumerator RotateCameraCoroutine(float angle)
    {
        isRotating = true;
        initialButton.SetActive(false);
        Quaternion startRotation = mainCamera.transform.rotation; // Initialrotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -angle, 0); //Target rotation
        float rotationProgress = 0f;
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / angle); //Normalize the rotation speed based on angle
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation,
                endRotation, rotationProgress); // Smoothly interpolate rotation
            yield return null;
        }
        mainCamera.transform.rotation = endRotation; // Ensure exact final rotation
        isRotating = false;
        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
    }

    public void Loadscene(string ExpositionScene)
    {
        StartCoroutine(scrollText);
        
        SceneManager.LoadScene("MainScene");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }


    private IEnumerator scrollText
    {
        get
        {
            panel.SetActive(true);
            textScroll.Scroll();
            yield return new WaitForSeconds(scrollLength);
            Loadscene("ExpositionScene");

        }
    }

    public void scrollingText()
    {
        StartCoroutine(scrollText);
        
    }


   

}
