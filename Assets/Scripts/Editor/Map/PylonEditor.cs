using Game.Common.Map.PylonMap;
using UnityEditor;
using UnityEngine;

namespace Game.Common.Map.Editor
{
    [CustomEditor(typeof(Pylon))]
    public class PylonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Pylon pylon = (Pylon)target;

            if (GUILayout.Button("Add keyframe"))
            {
                pylon.SetAnimationPosition();
            }
        }
    }
}
