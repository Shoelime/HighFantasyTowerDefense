using UnityEngine;

public class GemContainer : MonoBehaviour
{
    public Transform[] GemHolders { get; private set; }

    public void Initialize()
    {
        int childCount = transform.childCount;
        GemHolders = new Transform[childCount];
        Transform[] allTransforms = GetComponentsInChildren<Transform>();

        // add only the children transforms to the list  
        for (int i = 0; i < childCount; i++)
        {
            GemHolders[i] = allTransforms[i + 1];
        }
    }
}