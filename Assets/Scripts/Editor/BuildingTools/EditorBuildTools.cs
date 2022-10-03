using System.Diagnostics;
using System.IO;
using System.Linq;
using Game.Common.Settings;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.BuildingTools
{
    public class EditorBuildTools
    {

        [MenuItem("Build Debug/Build and run server and client")]
        static void BuildAndRunServerAndClient()
        {
            BuildAndRunServer();
            BuildAndRunClient();
        }
        
        [MenuItem("Build Debug/Build and run server")]
        static void BuildAndRunServer()
        {
            string exeLocation = "Builds/WindowsServer/SumoServer.exe";
            
            SetInitServer(true);
            
            BuildAndRun(exeLocation, BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.AllowDebugging, StandaloneBuildSubtarget.Player);
        }

        [MenuItem("Build Debug/Build and run client")]
        static void BuildAndRunClient()
        {
            string exeLocation = "Builds/WindowsClient/SumoClient.exe";
            
            SetInitServer(false);
            BuildAndRun(exeLocation, BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.AllowDebugging, StandaloneBuildSubtarget.Player);
        }
        
        [MenuItem("Build Debug/Editor as Server + 2 clients (debug server)")]
        static void BuildAndRunClient2()
        {
            BuildAndRunClient();
            string exeLocation = "Builds/WindowsClient/SumoClient.exe";
            StartFile(exeLocation);
            SetInitServer(true);
            EditorSceneManager.OpenScene("Assets/Scenes/InitScene.unity");
            EditorApplication.EnterPlaymode();
        }
        
        [MenuItem("Build Debug/Editor as Client + 1 c and s (debug client)")]
        static void DebugClient()
        {
            BuildAndRunServer();
            BuildAndRunClient();
            SetInitServer(false);
            EditorSceneManager.OpenScene("Assets/Scenes/InitScene.unity");
            EditorApplication.EnterPlaymode();
        }
        
        [MenuItem("Build Debug/Build Linux Server")]
        static void BuildLinuxServer()
        {
            string exeLocation = "Builds/LinuxServer/SumoServer.exe";
            
            SetInitServer(true);

            Build(exeLocation, BuildTarget.StandaloneLinux64, BuildOptions.Development | BuildOptions.AllowDebugging, StandaloneBuildSubtarget.Server);
        }
        
        [MenuItem("Build Debug/Build Mac Client")]
        static void BuildMacClient()
        {
            string exeLocation = "Builds/MacClient/SumoClient.exe";
            
            SetInitServer(false);

            Build(exeLocation, BuildTarget.StandaloneOSX, BuildOptions.Development | BuildOptions.AllowDebugging, StandaloneBuildSubtarget.Player);
        }
        
        [MenuItem("Build Debug/Init Server Flag (true)")]
        static void InitServerTrue()
        {
            SetInitServer(true);
        }

        [MenuItem("Build Debug/Init Server Flag (false)")]
        static void InitServerFalse()
        {
            SetInitServer(false);
        }

        static void BuildAndRun(string exeLocation, BuildTarget target, BuildOptions options, StandaloneBuildSubtarget subTarget)
        {

            var buildReport = Build(exeLocation, target, options, subTarget);

            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                StartFile(exeLocation);
            }
            else
            {
                Debug.LogError("Could not start build exe, build failed");
            }
            
        }


        static BuildReport Build(string exeLocation, BuildTarget target, BuildOptions options, StandaloneBuildSubtarget subTarget)
        {
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetSceneNames();
            buildPlayerOptions.locationPathName = exeLocation;
            buildPlayerOptions.target = target;
            buildPlayerOptions.options = options;
            buildPlayerOptions.subtarget = (int)subTarget;
            return BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        static void StartFile(string path)
        {
            Process.Start(Path.Combine(Application.dataPath, "../" + path));
        }
        
        static void SetInitServer(bool serverEnabled)
        {
            var masterSettings = AssetDatabase.LoadAssetAtPath<MasterSettings>("Assets/Scriptable Objects/Master Settings.asset");
            masterSettings.InitServer = serverEnabled;
            EditorUtility.SetDirty(masterSettings);
            SerializedObject obj = new SerializedObject(masterSettings);
            obj.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();    
        }

        static string[] GetSceneNames()
        {
            return EditorBuildSettings.scenes.Where(x => x.enabled)
                .Select(x => x.path).ToArray();
        }
    }
}
