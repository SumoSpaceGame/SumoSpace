using System;
using Game.Common.Networking;
using Game.Common.Settings;
using Game.Common.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Game.Common.UI.DirectConnection
{
    /// <summary>
    /// This class is a temporary handler for connecting and hosting servers.
    /// </summary>
    public class DirectConnectionUIHandler : MonoBehaviour
    {
        public TMP_InputField AddressTextField;
        public TMP_InputField PortTextField;  

        public MasterSettings masterSettings;
        
        private const string DEBUG_ServerAddress = "localhost";
        private const string DEBUG_HostServerAddress = "127.0.0.1";
        
        private string ServerAddress = "localhost";
        private ushort ServerPort = 22233;

        public GameObject enableWhenServer;

        
        private void Start()
        {
            ServerAddress = masterSettings.ServerAddress;
            ServerPort = masterSettings.ServerPort;
            
            if(PortTextField != null) PortTextField.text = "" + masterSettings.ServerPort;
           
            
            if (masterSettings.InitServer)
            {
                Debug.Log("Hosting server at localhost:" + masterSettings.ServerPort);
                FishNetConnector.Host(masterSettings.ServerPort);
                enableWhenServer.SetActive(true);
            }
        }

        public void ProcessTextFields()
        {
            ServerAddress = AddressTextField.text;
            ServerPort = UInt16.Parse(PortTextField.text);
        }
        
        public void ConnectToServer()
        {
            ProcessTextFields();
            FishNetConnector.Connect(ServerAddress, ServerPort);
        }

        /// <summary>
        /// Automatically polls the ip from sorrer.dev/sumospace/currentServer
        /// </summary>
        public void ConnectToServerAuto()
        {
            var ip = HttpUtil.Get("https://www.sorrer.dev/sumospace/ds.html");

            string[] split = ip.Split(':');
            if (split.Length > 2)
            {
                Debug.LogError("AutoServer invalid address");
                return;
            }
            
            Debug.Log("Auto to " + ip);

            string address = split[0];
            ushort port = UInt16.Parse(split[1]);
            
            FishNetConnector.Connect(address, port);
        }

        public void DebugConnect()
        {
            FishNetConnector.Connect(DEBUG_ServerAddress, masterSettings.ServerPort);
        }

        public void Host()
        {
            ProcessTextFields();
            FishNetConnector.Host(ServerPort);
        }
        
        public void DebugHost()
        {
            FishNetConnector.Host(masterSettings.ServerPort);
        }

    }
}
