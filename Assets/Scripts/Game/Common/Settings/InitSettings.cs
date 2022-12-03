using System;
using UnityEngine;

namespace Game.Common.Settings
{
    public class InitSettings : MonoBehaviour
    {

        public MasterSettings masterSettings;
        private void Awake()
        {
            var mapRegistry = masterSettings.mapRegistry;
            var matchSettings = masterSettings.matchSettings;
            
            matchSettings.SelectedMapItem = mapRegistry.GetMap(matchSettings.StartWithSelectedMapIndex);
                
            string[] args = Environment.GetCommandLineArgs ();
            string input = "";
            for (int i = 0; i < args.Length; i++) {
                if (i + 1 >= args.Length) continue;
                
                switch (args[i])
                {
                    case "-map":
                        var index = Int16.Parse(args[i + 1]);
                        
                        if (mapRegistry.HasMap(args[i + 1]))
                        {
                            matchSettings.SelectedMapItem = mapRegistry.GetMap(args[i + 1]);   
                        }else if (mapRegistry.HasMap(index))
                        {
                            matchSettings.SelectedMapItem = mapRegistry.GetMap(index);
                        }
                        else
                        {
                            Debug.LogError("Could not load from -map argument. Invalid map given. Defaulting 0");
                        }
                        break;
                    case "-port":
                        masterSettings.ServerPort = UInt16.Parse(args[i + 1]);
                        break;
                    case "-teamsize":
                        matchSettings.TeamSize = Int32.Parse(args[i + 1]);
                        break;
                    case "-teamamount":
                        matchSettings.TeamCount = Int32.Parse(args[i + 1]);
                        break;
                    case "-updateinterval":
                        // TODO: Add update interval
                        //network.updateInterval = UInt64.Parse(args[i + 1]);
                        break;
                    
                }
                
                
            }
        }
    }
}