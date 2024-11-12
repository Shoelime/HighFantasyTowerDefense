using System;
using UnityEngine;

public interface IInputManager : IUpdateableService
{
    Action<Vector3, GameObject> OnLeftMouseButton { get; set; }
    Action EscapeButton { get; set; }
    Vector2 MousePosition { get; }
}
