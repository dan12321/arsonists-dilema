using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the transitions between scenes
/// </summary>
public class SceneController : MonoBehaviour
{
    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;
    public string startingSceneName = "Glider";

    private bool isFading;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    private IEnumerator Start()
    {
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));

        StartCoroutine(Fade(0f));
    }

    /// <summary>
    /// Switches to a new scene
    /// </summary>
    /// <param name="sceneReaction">The reaction causing the scene change</param>
    public void LoadScene(SceneReaction sceneReaction)
    {
        if (!isFading)
            StartCoroutine(SwitchScenes(sceneReaction.sceneName));
    }

    /// <summary>
    /// Swaps out the scenes
    /// </summary>
    /// <param name="sceneName">The scene to change to</param>
    private IEnumerator SwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        BeforeSceneUnload?.Invoke();

        yield return SceneManager.UnloadSceneAsync(
            SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        AfterSceneLoad?.Invoke();

        yield return StartCoroutine(Fade(0f));
    }

    /// <summary>
    /// Loads in a new screen and sets it as the one to use
    /// </summary>
    /// <param name="sceneName">The scene to load</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    /// <summary>
    /// Fades in or out
    /// </summary>
    /// <param name="finalAlpha">The alpha value to fade to</param>
    /// <returns></returns>
    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(
                faderCanvasGroup.alpha,
                finalAlpha,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }
}
