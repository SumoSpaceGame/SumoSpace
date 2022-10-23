using System;
using Game.Common.Map.PylonMap;
using Game.Common.Phases.PhaseData;
using Game.Common.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Common.Map.Editor
{
    
    /// <summary>
    /// Map editor will allow you to easily -
    /// * Scrub through the timeline of the match (Make sure events work with this!)
    /// * Make connections and remove connections at the current time on the timeline
    /// * Animate and select pylons easily (Especially select, right now it is very hard to properly select a pylon)
    /// * Create and delete pylons
    /// * Set events to occur and x time
    ///
    /// The reason for doing a custom timeline, is to make it easier to use in the future
    /// We could use the regular unity timeline, but the way the maps work right now
    /// is through a custom time value. 
    /// </summary>
    public class MapEditor : EditorWindow
    {
        
        private GameMapManager manager;
        private bool usePercentage = false;

        private double currentMatchTimeMinutes = 0.0f;
        
        [MenuItem("Window/Map/GameMapManager")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MapEditor window = (MapEditor)EditorWindow.GetWindow(typeof(MapEditor), false, "Map Editor");
            window.Show();
        }

        private void OnGUI()
        {
            if (InitGameMap() == false)
            {
                GUILayout.Label("Can not load game map. Please make sure everything is made");
                if (GUILayout.Button("Create Pylon Map Objects"))
                {
                    CreateGameMap();
                }

                return;
            }
            
            
            
            if (manager.gameMap is PylonMap.PylonMap)
            {
                GUILayout.Label("Pylon map settings", EditorStyles.boldLabel);
                if (GUILayout.Button("Refresh pylon list"))
                {
                    ((PylonMap.PylonMap) manager.gameMap).ReloadPylonList();
                    ((PylonMap.PylonMap) manager.gameMap).RebuildInternalLists();
                }
                
            }

            usePercentage = EditorGUILayout.Toggle("Use Percentage", usePercentage);

            if (usePercentage)
            {
                currentMatchTimeMinutes = EditorGUILayout.DoubleField("Current Percentage", currentMatchTimeMinutes / manager.mapSettings.MatchTimeMinutes) * manager.mapSettings.MatchTimeMinutes;
                
                currentMatchTimeMinutes = (double)EditorGUILayout.Slider((float) currentMatchTimeMinutes / (float) manager.mapSettings.MatchTimeMinutes, 0.0f , 1.0f) * manager.mapSettings.MatchTimeMinutes;
            }
            else
            {
                currentMatchTimeMinutes = EditorGUILayout.DoubleField("Current Time", currentMatchTimeMinutes);
                
                manager.mapSettings.MatchTimeMinutes = EditorGUILayout.DoubleField("Max Match Time", manager.mapSettings.MatchTimeMinutes);

            }
            
            currentMatchTimeMinutes = Math.Clamp(currentMatchTimeMinutes, 0.0f,
                manager.mapSettings.MatchTimeMinutes);
            
            // TODO: Add scrubbing visualization
            
            manager.gameMap.UpdateMap(currentMatchTimeMinutes/manager.mapSettings.MatchTimeMinutes);

        }


        public bool InitGameMap()
        {
            if ((manager = Object.FindObjectOfType<GameMapManager>()) == null)
            {
                return false;
            }

            if ((manager.gameMap == null))
            {
                manager.gameMap = manager.GetComponent<IGameMap>();
                manager.gameMap.Init(false);

                if ((manager.gameMap) == null) return false;
            }

            return true;
        }

        /// <summary>
        /// Create all items needed for a game map
        /// Generates scriptable objects for the map too
        /// </summary>
        public void CreateGameMap()
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
            GameObject o = new GameObject("Game Map Manager");
            var sceneName = SceneManager.GetActiveScene().name;
            var gameMapManager = o.AddComponent<GameMapManager>();
            var pylonMap = o.AddComponent<PylonMap.PylonMap>();
            var pylonBuilder = o.AddComponent<PylonBuilder>();
            
            pylonMap.builder = pylonBuilder;
            gameMapManager.gameMap = pylonMap;
            gameMapManager.gameMapObject = pylonMap.gameObject;
            
            var assets = AssetDatabase.FindAssets("t:" + typeof(MasterSettings));

            if (assets.Length > 1)
            {
                DestroyImmediate(o);
                Debug.LogError("Could not create game map objects, master settings not found");
                return;
            }

            if (assets.Length == 0)
            {
                DestroyImmediate(o);
                Debug.LogError("Could not find master settings. (This should not be a problem)");
                return;
            }

            var folder = GetFolder(new string[]{"Scriptable Objects", "Data", "Maps", sceneName});

            var mapSettings = ScriptableObject.CreateInstance<MapSettings>();

            var mapSettingsLoc = folder + "/" + sceneName +
                                 " Settings.asset";
            //Set map settings data
            mapSettings.MatchTimeMinutes = 30;
            AssetDatabase.CreateAsset(mapSettings,
                AssetDatabase.GenerateUniqueAssetPath(mapSettingsLoc));

            
            mapSettings = AssetDatabase.LoadAssetAtPath<MapSettings>(mapSettingsLoc);

            //Create map entry
            var masterSettings =  AssetDatabase.LoadAssetAtPath<MasterSettings>(AssetDatabase.GUIDToAssetPath(assets[0]));
            
            gameMapManager.masterSettings = masterSettings;
            gameMapManager.mapSettings = mapSettings;
            
            masterSettings.mapRegistry.AddMapItem(new MapRegistry.MapItem()
            {
                sceneName = sceneName,
                mapSettings = mapSettings
            });

            
            EditorUtility.SetDirty(mapSettings);
            EditorUtility.SetDirty(masterSettings.mapRegistry);
            
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        public string GetFolder(string[] folders)
        {
            
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

            return currentFolderStruct;

        }
    }
    
}