using System;
using Game.Common.Networking;
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
        
        private const string DEBUG_ServerAddress = "localhost";
        private const ushort DEBUG_ServerPort = 22233;
        private const string DEBUG_HostServerAddress = "127.0.0.1";
        private const ushort DEBUG_HostServerPort = 22233;
        
        private string ServerAddress = "localhost";
        private ushort ServerPort = 22233;
        
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
            connector.Connect(DEBUG_ServerAddress, DEBUG_ServerPort);
        }

        public void Host()
        {
            ProcessTextFields();
            connector.Host(ServerAddress, ServerPort);
        }
        
        public void DebugHost()
        {
            connector.Host(DEBUG_HostServerAddress, DEBUG_HostServerPort);
        }
    }
}
