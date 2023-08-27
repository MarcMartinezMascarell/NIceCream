using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// <summary>
    /// Unity method for drawing GUI in Editor.
    /// </summary>
    /// <param name="position">Position of the property.</param>
    /// <param name="property">Property to draw.</param>
    /// <param name="label">Label of the property.</param>
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Saving previous GUI enabled value.
        var previousGUIState = GUI.enabled;
        //Disabling edit for property.
        GUI.enabled = false;
        //Drawing Property.
        EditorGUI.PropertyField(position, property, label);
        //Setting old GUI enabled value.
        GUI.enabled = previousGUIState;
    }
}
