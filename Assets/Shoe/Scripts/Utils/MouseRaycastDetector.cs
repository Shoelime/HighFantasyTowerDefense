using UnityEngine;

public class MouseRaycastDetector
{
    private RaycastLayerData raycastLayerData;

    public GameObject CurrentMouseTarget { get; private set; }

    public void Initialize()
    {
        RaycastLayerData[] allRaycastLayerData = Resources.LoadAll<RaycastLayerData>("");
        raycastLayerData = allRaycastLayerData[0];
    }

    public void CheckMouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, raycastLayerData.LayersToDetect))
        {
            CurrentMouseTarget = hitInfo.collider.gameObject;
        }
        else
        {
            CurrentMouseTarget = null;
        }
    }
}