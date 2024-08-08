using UnityEngine;
using UnityEngine.UI;

public class LoadNextLevelButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadNextScene);
    }

    void LoadNextScene()
    {
        if (Loader.GetCurrentScene() == Loader.Scene.MainMenu)
            Loader.Load(Loader.Scene.Level01);
        else if (Loader.GetCurrentScene() == Loader.Scene.Level01)
            Loader.Load(Loader.Scene.Level02);
        else if (Loader.GetCurrentScene() == Loader.Scene.Level02)
            Loader.Load(Loader.Scene.MainMenu);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
