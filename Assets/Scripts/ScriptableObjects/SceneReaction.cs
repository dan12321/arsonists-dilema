using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The interaction reaction to change scene
/// </summary>
[CreateAssetMenu()]
public class SceneReaction : ScriptableObject
{
    public string sceneName;

    private SceneController sceneController;

    /// <summary>
    /// Set up when the scene loads
    /// </summary>
    private void OnEnable()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    /// <summary>
    /// Changes to the new scene
    /// </summary>
    public void ChangeScene()
    {
        sceneController.LoadScene(this);
    }
}
