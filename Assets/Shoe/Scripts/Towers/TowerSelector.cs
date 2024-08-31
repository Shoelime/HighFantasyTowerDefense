using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    [SerializeField] private TowerAI towerController;

    private void OnEnable()
    {
        Services.Get<IInputManager>().OnLeftMouseButton += ButtonPressed;
    }

    private void ButtonPressed(Vector3 clickPosition, GameObject clickObject)
    {
        towerController.ButtonPressed(clickPosition, towerController.gameObject);
    }

    private void OnDisable()
    {
        Services.Get<IInputManager>().OnLeftMouseButton -= ButtonPressed;
    }
}
