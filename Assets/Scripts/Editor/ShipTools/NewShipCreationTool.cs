using Game.Common.Gameplay.Ship;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace UnityTemplateProjects.Editor.ShipTools
{
    public class NewShipCreationTool: EditorWindow
    {
        private string _shipName;
        [SerializeField] private ScriptableObject defaultMovement;
        [SerializeField] private GameObject defaultShipPrefab;
        private GameObject _representationPrefab;
        
        [MenuItem("Ship Utils/Create New Ship")]
        private static void CreateNewShip()
        {
            var window = GetWindow(typeof(NewShipCreationTool));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Ship Creation Wizard 0.1a");
            _shipName = EditorGUILayout.TextField("Ship Name", _shipName);
            _representationPrefab = EditorGUILayout.ObjectField(
                "3D Representation Prefab (Optional)", 
                _representationPrefab, 
                typeof(GameObject), 
                false) as GameObject;
            if (GUILayout.Button("Create Ship"))
            {
                CreateShipInfrastructure();
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
            

            var movement = Instantiate(defaultMovement) as ShipMovement;//CreateInstance<ShipMovement>();
            var movementPath = $"Assets/Scriptable Objects/Loadouts/Controls/{_shipName}Movement.asset";
            AssetDatabase.CreateAsset(movement, movementPath);
            
            var loadout = CreateInstance<ShipLoadout>();
            var loadoutPath = $"Assets/Scriptable Objects/Loadouts/{_shipName}Loadout.asset";
            loadout.ShipMovement = movement;
            AssetDatabase.CreateAsset(loadout, loadoutPath);

            var prefabPath = $"Assets/Prefabs/Ships/{_shipName}Ship.prefab";
            var shipPrefab = PrefabUtility.InstantiatePrefab(defaultShipPrefab) as GameObject;
            Debug.Assert(shipPrefab != null, nameof(shipPrefab) + " != null");
            shipPrefab.GetComponent<ShipManager>().shipLoadout = loadout;
            if (_representationPrefab is not null)
            {
                shipPrefab.GetComponent<SimulationObject>().representative = _representationPrefab;
            }
            PrefabUtility.SaveAsPrefabAsset(shipPrefab, prefabPath);
            DestroyImmediate(shipPrefab);
            
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}