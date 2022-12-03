using System;
using Game.Common.Map.PylonMap;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Common.Map.Editor
{
    [CustomEditor(typeof(Pylon))]
    public class PylonEditor : UnityEditor.Editor
    {
        public static Pylon CurrentlySelectedPylon;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Pylon pylon = (Pylon)target;

            EditorGUI.BeginDisabledGroup(pylon.pylonAnimation != null);
            
            if (GUILayout.Button("Generate Pylon Animation") && pylon.pylonAnimation == null)
            {
                
                var folders = new string[]{"Scriptable Objects", "Data", "Maps", SceneManager.GetActiveScene().name, "PylonData"};

                string currentFolderStruct = "Assets";

                for (int i = 0; i < folders.Length; i++)
                {
                    if (AssetDatabase.IsValidFolder(currentFolderStruct + "/"+ folders[i]))
                    {
                        currentFolderStruct += "/" + folders[i];
                        continue;
                    }

                    AssetDatabase.CreateFolder(currentFolderStruct,folders[i]);

                    currentFolderStruct += "/" + folders[i];
                }
                
                
                var uniqueName = AssetDatabase.GenerateUniqueAssetPath("Assets/Scriptable Objects/Data/Maps/" +
                                                                       SceneManager.GetActiveScene().name +
                                                                       "/PylonData/Pylon Animation.asset");
                if (uniqueName != "")
                {
                    
                    PylonAnimation asset = ScriptableObject.CreateInstance<PylonAnimation>();
                    AssetDatabase.CreateAsset(asset, uniqueName);

                    pylon.pylonAnimation = asset;
                    asset.CurrentGameobject = pylon;
                    
                    Debug.Log("Generated new pylon animation at " + uniqueName);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                }
                else
                {
                    Debug.LogError("Failed to generate path structure for new pylon animation asset");
                }
            }
            
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(pylon.pylonAnimation == null);
            
            if (GUILayout.Button("Add keyframe"))
            {
                pylon.SetAnimationPosition();
            }
            
            /* Disabled to prevent accidental deletions
            if (GUILayout.Button("Delete Animation"))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(pylon.pylonAnimation));
            }
            */
            
            EditorGUI.EndDisabledGroup();
        }
    }
}
