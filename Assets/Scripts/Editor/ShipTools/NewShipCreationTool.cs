using Game.Common.Gameplay.Ship;
using UnityEditor;
using UnityEngine;

namespace Editor.ShipTools
{
    public class NewShipCreationTool: EditorWindow
    {
        private const string Version = "0.1a";
        
        private string _shipName;
        [SerializeField] private ScriptableObject defaultMovement;
        [SerializeField] private GameObject defaultShipPrefab;
        [SerializeField] private ScriptableObject shipList;
        private GameObject _representationPrefab;
        
        [MenuItem("Ship Utils/Create New Ship")]
        private static void CreateNewShip()
        {
            var window = GetWindow(typeof(NewShipCreationTool));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField($"Ship Creation Wizard {Version}");
            _shipName = EditorGUILayout.TextField("Ship Name", _shipName);
            _representationPrefab = EditorGUILayout.ObjectField(
                "3D Representation Prefab (Optional)", 
                _representationPrefab, 
                typeof(GameObject), 
                false) as GameObject;
            if (GUILayout.Button("Create Ship"))
            {
                CreateShipInfrastructure();
                this.Close();
            }
        }

        private void CreateShipInfrastructure()
        {
            // Create ship movement (from default)
            // Create Loadout
            // Add movement to Loadout
            // Create Ship Prefab
            // Rename Prefab
            // Attach representation to prefab (if applicable)
            // Attach ship loadout to prefabs

            var commonShipPath = $"Assets/Scriptable Objects/Loadouts/{_shipName}";

            CreateNewFolder("Assets/Scriptable Objects/Loadouts", _shipName);

            var movementPath = $"{commonShipPath}/{_shipName}Movement.asset";
            ShipMovement movement;
            if (!AssetDatabase.AssetPathExists(movementPath))
            {
                Debug.Log("Creating new ShipMovement");
                movement = Instantiate(defaultMovement) as ShipMovement; //CreateInstance<ShipMovement>();
                AssetDatabase.CreateAsset(movement, movementPath);
            }
            else
            {
                Debug.Log("ShipMovement exists - Fetching");
                movement = AssetDatabase.LoadAssetAtPath<ShipMovement>(movementPath);
                if (movement is null)
                {
                    Debug.LogError("Failed to fetch ShipMovement - Aborting");
                    return;
                }
            }

            var loadoutPath = $"{commonShipPath}/{_shipName}Loadout.asset";
            ShipLoadout loadout;
            if (!AssetDatabase.AssetPathExists(loadoutPath))
            { 
                Debug.Log("Creating new ShipLoadout");
                loadout = CreateInstance<ShipLoadout>();
                loadout.InitList();
                loadout.ShipMovement = movement;
                AssetDatabase.CreateAsset(loadout, loadoutPath);
            }
            else
            {
                Debug.Log("ShipLoadout exists - Fetching");
                loadout = AssetDatabase.LoadAssetAtPath<ShipLoadout>(loadoutPath);
                if (loadout is null)
                {
                    Debug.LogError("Failed to fetch ShipLoadout - Aborting");
                    return;
                }
            }

            var prefabPath = $"Assets/Prefabs/Ships/{_shipName}Ship.prefab";
            GameObject savedPrefab;
            if (!AssetDatabase.AssetPathExists(prefabPath))
            {
                Debug.Log("Creating new Ship Prefab");
                var shipPrefab = PrefabUtility.InstantiatePrefab(defaultShipPrefab) as GameObject;
                Debug.Assert(shipPrefab != null, nameof(shipPrefab) + " != null");
                shipPrefab.GetComponent<ShipManager>().shipLoadout = loadout;
                if (_representationPrefab is not null)
                {
                    shipPrefab.GetComponent<SimulationObject>().representative = _representationPrefab;
                }
                savedPrefab = PrefabUtility.SaveAsPrefabAsset(shipPrefab, prefabPath);
                DestroyImmediate(shipPrefab);
            }
            else
            {
                Debug.Log("Ship Prefab exists - Fetching");
                savedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (savedPrefab is null)
                {
                    Debug.LogError("Failed to fetch Ship Prefab - Aborting");
                    return;
                }
            }

            if ((shipList as ShipPrefabList)?.GetAllShips().Contains(savedPrefab) ?? false)
            {
                (shipList as ShipPrefabList)?.AddShip(savedPrefab);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var shipScriptsPath = "Assets/Scripts/Game/Ships";

            CreateNewFolder(shipScriptsPath, _shipName);
            CreateNewFolder($"{shipScriptsPath}/{_shipName}", "Client");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}/Client", "Behaviours");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}/Client", "Commands");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}", "Common");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}/Common", "Abilities");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}", "Server");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}/Server", "Behaviours");
            CreateNewFolder($"{shipScriptsPath}/{_shipName}/Server", "Commands");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            /*
             * Ships
             * |-ShipName
             * |    |-Client
             * |    |    |-Behaviours
             * |    |    |-Commands
             * |    |-Common
             * |    |    |-Abilities
             * |    |-Server
             * |    |    |-Behaviours
             * |    |    |-Commands
            */
            
            EditorUtility.FocusProjectWindow();
        }

        private static string CreateNewFolder(string parentFolder, string folderName)
        {
            if(!AssetDatabase.IsValidFolder($"{parentFolder}/{folderName}"))
            {
                Debug.Log($"Creating folder {parentFolder}/{folderName}");
                return AssetDatabase.CreateFolder(parentFolder, folderName);
            }
            Debug.Log($"Folder {parentFolder}/{folderName} already exists - Skipping");
            return null;
        }
    }
}