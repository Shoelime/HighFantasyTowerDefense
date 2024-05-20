using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder : IService
{
    public GameObject[] Waypoints { get; }
    public Transform EntrancePoint { get; }
}
