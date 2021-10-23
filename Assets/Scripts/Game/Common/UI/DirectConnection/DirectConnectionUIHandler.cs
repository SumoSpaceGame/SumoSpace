using System;
using Game.Common.Networking;
using Game.Common.Settings;
using TMPro;
using UnityEngine;
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

        public BasicNetworkConnector connector;
        public MasterSettings masterSettings;
        
        private const string DEBUG_ServerAddress = "localhost";
        private const string DEBUG_HostServerAddress = "127.0.0.1";
        
        private string ServerAddress = "localhost";
        private ushort ServerPort = 22233;


        private void Start()
        {
            if (masterSettings.InitServer)
            {
                Debug.Log("Hosting server at localhost:" + masterSettings.ServerPort);
                connector.Host("localhost", masterSettings.ServerPort);
            }

            ServerAddress = masterSettings.ServerAddress;
            ServerPort = masterSettings.ServerPort;
            
            PortTextField.text = "" + masterSettings.ServerPort;
        }

        public void ProcessTextFields()
        {
            ServerAddress = AddressTextField.text;
            ServerPort = UInt16.Parse(PortTextField.text);
        }
        
        public void ConnectToServer()
        {
            ProcessTextFields();
            connector.Connect(ServerAddress, ServerPort);
        }

        public void DebugConnect()
        {
            connector.Connect(DEBUG_ServerAddress, masterSettings.ServerPort);
        }

        public void Host()
        {
            ProcessTextFields();
            connector.Host(ServerAddress, ServerPort);
        }
        
        public void DebugHost()
        {
            connector.Host(DEBUG_HostServerAddress, masterSettings.ServerPort);
        }
    }
}
