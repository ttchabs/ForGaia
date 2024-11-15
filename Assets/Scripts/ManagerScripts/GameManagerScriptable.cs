using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Managers", menuName = "GameManager")]
public class GameManagerScriptable : ScriptableObject
{
    [SerializeField] string[] _sceneNames;

    public string[] SceneNames => _sceneNames;
}
