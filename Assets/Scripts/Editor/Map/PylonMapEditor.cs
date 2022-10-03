using System.Collections.Generic;
using System.Linq;
using Game.Common.Map.PylonMap;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;



namespace Game.Common.Map.Editor
{
    [CustomEditor(typeof(PylonMap.PylonMap))]
    public class PylonMapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PylonMap.PylonMap pylonMap = (PylonMap.PylonMap)target;

            if (GUILayout.Button("Replace list with scene pylons"))
            {
                pylonMap.ReloadPylonList();

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}