using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Common.Gameplay.Abilities;
using Game.Common.Gameplay.Ship;
using Game.Common.Util;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Editor.ShipTools
{
    /**
     * TODO: Manifest system
     * Creating an ability should add it to a list that is unique per ship so that abilities can
     * be dynamically retrieved at a later date
     *
     * 
     */
    public class NewAbilityCreationTool : EditorWindow
    {
        private const string Version = "0.1a";
        private List<GameObject> _allShips;
        [SerializeField] private ScriptableObject shipList;
        
        private int _selectedShip;
        private ShipLoadout.AbilityType _loadoutSlot;
        private string _abilityName = "";
        private bool _clientSideRenderable = true;
        
        [MenuItem("Ship Utils/Create New Ability")]
        private static void CreateNewShip()
        {
            var window = GetWindow(typeof(NewAbilityCreationTool));
            window.Show();
        }

        private string[] FetchShips()
        {
            var list = (shipList as ShipPrefabList)?.GetAllShips().Where(x => x is not null).ToList();
            _allShips = list;
            return (list ?? new List<GameObject>()).Select(x => x.name).ToArray();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField($"Ability Creation Wizard {Version}");
            _selectedShip = EditorGUILayout.Popup("Ship", _selectedShip, _allShips is null ? FetchShips() : 
                _allShips.Select(x => x.name).ToArray());
            _loadoutSlot = (ShipLoadout.AbilityType)EditorGUILayout.EnumPopup("Loadout Slot", _loadoutSlot);
            _abilityName = EditorGUILayout.TextField("Ability Name", _abilityName);
            _abilityName = _abilityName.Replace(" ", "");
            _clientSideRenderable = EditorGUILayout.Toggle(
                new GUIContent("Client Side Renderable", "Checked if the ability renders on the Client side"),
                _clientSideRenderable);
            
            if (GUILayout.Button("Create Ability Infrastructure"))
            {
                CreateAbilityInfrastructure();
            }

            if (GUILayout.Button("Force Recompile"))
            {
                ForceRefreshAndRecompile();
            }
            
            GUILayout.Label("Make sure Unity has recompiled BEFORE linking!");
            if (GUILayout.Button("Link Ability"))
            {
                LinkAbility();
            }
        }

        private void CreateAbilityInfrastructure()
        {
            var shipName = _allShips[_selectedShip].name.RemoveSuffix("Ship");
            var commonPathPrefix = $"Assets/Scripts/Game/Ships/{shipName}";
            
            
            // Generate Ship Ability 
            var abilityTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    "Assets/Scripts/Editor/ShipTools/Script Templates/AbilityScriptTemplate.txt");
            var abilityPath = $"{commonPathPrefix}/Common/Abilities/";
            var abilityFilename = $"{shipName}{_abilityName}Ability.cs";
            GenerateScript(abilityTemplate, 
                abilityPath, 
                abilityFilename);
            
            // Generate ClientBehaviour
            var clientBehaviourTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    "Assets/Scripts/Editor/ShipTools/Script Templates/ClientBehaviourScriptTemplate.txt");
            var clientBehaviourPath = $"{commonPathPrefix}/Client/Behaviours/";
            var clientBehaviourFilename = $"{shipName}{_abilityName}ClientBehaviour.cs";
            GenerateScript(clientBehaviourTemplate, 
                clientBehaviourPath, 
                clientBehaviourFilename);

            // Generate ServerBehaviour
            var serverBehaviourTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    "Assets/Scripts/Editor/ShipTools/Script Templates/ServerBehaviourScriptTemplate.txt");
            GenerateScript(serverBehaviourTemplate, 
                $"{commonPathPrefix}/Server/Behaviours/", 
                $"{shipName}{_abilityName}ServerBehaviour.cs");
            
            
            // Generate ClientCommand
            var clientCommandTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    "Assets/Scripts/Editor/ShipTools/Script Templates/ClientCommandScriptTemplate.txt");
            GenerateScript(clientCommandTemplate, 
                $"{commonPathPrefix}/Client/Commands/", 
                $"{shipName}{_abilityName}ClientCommand.cs");
            
            // Generate ServerCommand
            var serverCommandTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>(
                    "Assets/Scripts/Editor/ShipTools/Script Templates/ServerCommandScriptTemplate.txt");
            GenerateScript(serverCommandTemplate, 
                $"{commonPathPrefix}/Server/Commands/", 
                $"{shipName}{_abilityName}ServerCommand.cs");
            
            // Compile our new scripts
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            CompilationPipeline.RequestScriptCompilation();
        }

        private void LinkAbility()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            var shipName = _allShips[_selectedShip].name.RemoveSuffix("Ship");
            
            var abilityType = Type.GetType($"Game.Ships.{shipName}.Common.Abilities.{shipName}{_abilityName}Ability, Game");
            var serverBehaviourType =
                Type.GetType($"Game.Ships.{shipName}.Server.Behaviours.{shipName}{_abilityName}ServerBehaviour, Game");
            var clientBehaviourType =
                Type.GetType($"Game.Ships.{shipName}.Client.Behaviours.{shipName}{_abilityName}ClientBehaviour, Game");

            if (abilityType is null || serverBehaviourType is null || clientBehaviourType is null)
            {
                Debug.LogError("Failed to get one or more types, check Ability and Behaviour scripts are properly compiled and not malformed - Aborting");
            }
            
            var behaviourPrefabCommonPath = "Assets/Prefabs/AbilityBehaviours/";

            var serverBehaviourPrefabPath =
                $"{behaviourPrefabCommonPath}Server/{shipName}{_abilityName}ServerBehaviour.prefab";
            GameObject serverBehaviourPrefab;
            if (!AssetDatabase.AssetPathExists(serverBehaviourPrefabPath))
            {
                Debug.Log("Creating new Server Behaviour Prefab");
                var serverBehaviourObject = new GameObject($"{shipName}{_abilityName}ServerBehaviour");
                ObjectFactory.AddComponent(serverBehaviourObject, serverBehaviourType);
                serverBehaviourPrefab = PrefabUtility.SaveAsPrefabAsset(serverBehaviourObject, serverBehaviourPrefabPath);
                DestroyImmediate(serverBehaviourObject);
            }
            else
            {
                Debug.Log("Server Behaviour Prefab exists - Fetching");
                serverBehaviourPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(serverBehaviourPrefabPath);
                if (serverBehaviourPrefab is null)
                {
                    Debug.LogError("Failed to fetch Server Behaviour Prefab - Aborting");
                    return;
                }
            }

            var clientBehaviourPrefabPath =
                $"{behaviourPrefabCommonPath}Client/{shipName}{_abilityName}ClientBehaviour.prefab";
            GameObject clientBehaviourPrefab;
            if (!AssetDatabase.AssetPathExists(clientBehaviourPrefabPath))
            {
                Debug.Log("Creating new Client Behaviour Prefab");
                var clientBehaviourObject = new GameObject($"{shipName}{_abilityName}ClientBehaviour");
                ObjectFactory.AddComponent(clientBehaviourObject, clientBehaviourType);
                clientBehaviourPrefab = PrefabUtility.SaveAsPrefabAsset(clientBehaviourObject,
                    clientBehaviourPrefabPath);
                DestroyImmediate(clientBehaviourObject);
            }
            else
            {
                Debug.Log("Client Behaviour Prefab exists - Fetching");
                clientBehaviourPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(clientBehaviourPrefabPath);
                if (clientBehaviourPrefab is null)
                {
                    Debug.LogError("Failed to fetch Client Behaviour Prefab - Aborting");
                    return;
                }
            }

            var abilitySOPath = $"Assets/Scriptable Objects/Loadouts/{shipName}/{shipName}{_abilityName}.asset";
            ShipAbility abilitySO;
            if (!AssetDatabase.AssetPathExists(abilitySOPath))
            {
                Debug.Log("Creating new Ability Scriptable Object");
                abilitySO = CreateInstance(abilityType) as ShipAbility;

                abilitySO.SetServerBehaviour(serverBehaviourPrefab);
                abilitySO.SetClientBehaviour(clientBehaviourPrefab);

                var shipLoadout = _allShips[_selectedShip].GetComponent<ShipManager>().shipLoadout;
                shipLoadout.AddAbility(abilitySO, _loadoutSlot);

                AssetDatabase.CreateAsset(abilitySO, abilitySOPath);
            }
            else
            {
                Debug.Log("Ability Scriptable Object exists - Fetching");
                abilitySO = AssetDatabase.LoadAssetAtPath<ShipAbility>(abilitySOPath);
                if (abilitySO is null)
                {
                    Debug.LogError("Failed to fetch Ability Scriptable Object - Aborting");
                    return;
                }
                abilitySO.SetServerBehaviour(serverBehaviourPrefab);
                abilitySO.SetClientBehaviour(clientBehaviourPrefab);
                var shipLoadout = _allShips[_selectedShip].GetComponent<ShipManager>().shipLoadout;
                shipLoadout.AddAbility(abilitySO, _loadoutSlot);
            }
        }

        private void GenerateScript(TextAsset template, string destination, string filename)
        {
            if (AssetDatabase.LoadAssetAtPath<MonoBehaviour>(destination + filename) is not null)
            {
                Debug.Log($"Script {destination + filename} already exists - Skipping");
                return;
            }
            var scriptContents = template.text;
            var shipName = _allShips[_selectedShip].name.RemoveSuffix("Ship");
            scriptContents = scriptContents.Replace("#SHIP_NAME#", shipName);
            scriptContents = scriptContents.Replace("#ABILITY_NAME#", _abilityName);
            scriptContents = scriptContents.Replace("#ABILITY_SLOT#", _loadoutSlot.ToString());
            scriptContents = scriptContents.Replace("#RENDERABLE#", _clientSideRenderable ? "Renderable" : "");
            
            using(var sw = new StreamWriter(destination + filename)) {
                sw.Write(scriptContents);
                sw.Close();
            }
            AssetDatabase.ImportAsset(destination + filename, ImportAssetOptions.ForceSynchronousImport);
            Debug.Log($"Created script {filename}");
        }

        private void ForceRefreshAndRecompile()
        {
            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}

