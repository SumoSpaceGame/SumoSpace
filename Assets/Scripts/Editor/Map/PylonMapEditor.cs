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
                Pylon[] obj = (Pylon[]) Object.FindObjectsOfType(typeof (Pylon), true);
                foreach (var pylon in obj)
                {
                    pylon.map = pylonMap;
                }
                pylonMap.pylons = obj.ToList();

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}