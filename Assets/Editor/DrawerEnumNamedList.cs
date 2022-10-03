using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(EnumNamedListAttribute))]
public class DrawerEnumNamedList : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var enumNames = attribute as EnumNamedListAttribute;
        var index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
        label.text = index < enumNames.names.Length ? enumNames.names[index] : $"[Index {index}]";
        EditorGUI.PropertyField( position, property, label, true );
    }
}