using UnityEditor;
using UnityEngine;

namespace Game.Common.Map.Editor
{
    [CustomEditor(typeof(MapRegistry))]
    public class MapRegistryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Remove invalid"))
            {
                ((MapRegistry) target).CleanUpList();
            }
        }
    }
}