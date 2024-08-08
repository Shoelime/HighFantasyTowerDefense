using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    /// <summary>
    /// Used as a placeholder class to run the coroutine thats otherwise inaccessible to this class
    /// </summary>
    public class CoroutineMonoBehavior : MonoBehaviour { }

    public enum Scene 
    { 
        Level01, 
        Level02, 
        LoadingScreen, 
        MainMenu 
    }

    public static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene)
    {
        /// Load the next scene asyncronously in the background
        /// 
        onLoaderCallback = () => 
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<CoroutineMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));
        };

        /// Display loading screen while we load the actual screne
        /// 
        SceneManager.LoadSceneAsync(Scene.LoadingScreen.ToString());
    }

    public static void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name.ToString();
        Scene scene = Scene.MainMenu;

        if (currentSceneName.Contains("01"))
            scene = Scene.Level01;
        else if (currentSceneName.Contains("02"))
            scene = Scene.Level02;

        /// Load the next scene asyncronously in the background
        /// 
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<CoroutineMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));
        };

        /// Display loading screen while we load the actual screne
        /// 
        SceneManager.LoadSceneAsync(Scene.LoadingScreen.ToString());
    }

    public static Scene GetCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name.ToString();

        if (currentSceneName.Contains("Main"))
            return Scene.MainMenu;
        else if (currentSceneName.Contains("01"))
            return Scene.Level01;
        else if (currentSceneName.Contains("02"))
            return Scene.Level02;

        return default;
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else return 1f;
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
