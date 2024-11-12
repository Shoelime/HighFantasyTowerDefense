#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector layout first
        DrawDefaultInspector();

        // Get the target object (the script instance)
        var targetObject = target as MonoBehaviour;

        // Get all methods of the target object
        var methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Iterate over all methods to find ones with the [Button] attribute
        foreach (var method in methods)
        {
            // Check if the method has the [Button] attribute
            var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
            if (attributes.Length > 0)
            {
                if (GUILayout.Button(method.Name))
                {
                    // Invoke the method when the button is pressed
                    method.Invoke(targetObject, null);
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ButtonAttribute : PropertyAttribute { }
#endif