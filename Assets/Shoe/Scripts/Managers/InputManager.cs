using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : IInputManager
{
    private MouseRaycastDetector mouseRaycastDetector;

    public Action<Vector3, GameObject> OnLeftMouseButton { get; set; }
    public Action EscapeButton { get; set; }
    public Vector2 MousePosition { get; private set; }

    public void Initialize()
    {
        mouseRaycastDetector =  new MouseRaycastDetector();
        mouseRaycastDetector.Initialize();
    }

    void IUpdateableService.Update()
    {
        MousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                OnLeftMouseButton?.Invoke(Input.mousePosition, mouseRaycastDetector.CurrentMouseTarget);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeButton?.Invoke();

        if (Services.Get<IGameStateHandler>().GetCurrentGameState == GameState.Paused)
            return;

        mouseRaycastDetector.CheckMouseRaycast();
    }
}