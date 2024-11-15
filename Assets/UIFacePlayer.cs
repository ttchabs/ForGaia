using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacePlayer : MonoBehaviour
{
    public Transform camToFace;

    void Start()
    {
        camToFace = FirstPersonControls.Instance.playerCamera.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(camToFace.position + camToFace.forward);
    }
}
