using System;
using UnityEngine;

using Unity.Multiplayer.Playmode;

namespace Game.Common.Settings
{
    public class InitSettings : MonoBehaviour
    {

        public MasterSettings masterSettings;
        private void Awake()
        {


            masterSettings.ClientDebugAutoConnect = false;

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
                    case "-autoconnect":
                        // TODO: Temporary way to auto connect
                        masterSettings.ClientDebugAutoConnect = true;
                        break;
                    
                }

#if UNITY_EDITOR
                if (CurrentPlayer.Tag != "")
                {
                    var tag = CurrentPlayer.Tag.ToLower();

                
                    if (tag.Contains("server"))
                    {
                        masterSettings.InitServer = true;
                    }

                    if (tag.Contains("player"))
                    {
                        masterSettings.ClientDebugAutoConnect = true;
                    }
                }
#endif
            }
        }
    }
}