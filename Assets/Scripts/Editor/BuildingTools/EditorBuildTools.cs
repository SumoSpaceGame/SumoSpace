using System.Diagnostics;
using System.IO;
using System.Linq;
using Game.Common.Settings;
using UnityEditor;
using UnityEditor.Build.Reporting;
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
            
            BuildAndRun(exeLocation, BuildTarget.StandaloneWindows64, BuildOptions.EnableHeadlessMode | BuildOptions.Development | BuildOptions.AllowDebugging);
        }

        [MenuItem("Build Debug/Build and run client")]
        static void BuildAndRunClient()
        {
            string exeLocation = "Builds/WindowsClient/SumoClient.exe";
            
            SetInitServer(false);
            BuildAndRun(exeLocation, BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.AllowDebugging);
        }
        
        [MenuItem("Build Debug/Build Linux Server")]
        static void BuildAndRunLinuxServer()
        {
            string exeLocation = "Builds/WindowsServer/SumoServer.exe";
            
            SetInitServer(true);

            BuildAndRun(exeLocation, BuildTarget.StandaloneLinux64, BuildOptions.EnableHeadlessMode | BuildOptions.Development | BuildOptions.AllowDebugging);
        }
        
        static void BuildAndRun(string exeLocation, BuildTarget target, BuildOptions options)
        {

            var buildReport = Build(exeLocation, target, options);

            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                StartFile(exeLocation);
            }
            else
            {
                Debug.LogError("Could not start build exe, build failed");
            }
            
        }


        static BuildReport Build(string exeLocation, BuildTarget target, BuildOptions options)
        {
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GetSceneNames();
            buildPlayerOptions.locationPathName = exeLocation;
            buildPlayerOptions.target = target;
            buildPlayerOptions.options = options;

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
            AssetDatabase.SaveAssets();
        }

        static string[] GetSceneNames()
        {
            return EditorBuildSettings.scenes.Where(x => x.enabled)
                .Select(x => x.path).ToArray();
        }
    }
}
