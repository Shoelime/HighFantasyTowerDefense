using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int currentLevel = 0;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image loaderBar;

    public bool IsShuttingDown { get; private set; } = false;
    public bool IsChangingScene { get; private set; } = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    private void Update()
    {

    }

    async void LoadNextLevel()
    {
        var scene = SceneManager.LoadSceneAsync(currentLevel++);
        scene.allowSceneActivation = false;

        loaderBar.fillAmount = 0;
        loaderCanvas.SetActive(true);

        IsChangingScene = true;

        do
        {
            /// Just to see if this works
            await Task.Delay(100);

            loaderBar.fillAmount = scene.progress;
        } while (scene.progress < 0.9f);

        IsChangingScene = false;

        scene.allowSceneActivation = true;
        loaderCanvas.SetActive(false);
    }

    void RestartLevel()
    {

    }

    private void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
}
